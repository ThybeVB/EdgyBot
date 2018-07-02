using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore;
using EdgyBot.Database;

namespace EdgyBot.Modules.Categories
{
    [Name("Utility Commands"), Summary("Utility Commands... What else can i say?")]
    public class UtilityCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("purge", RunMode = RunMode.Async), Name("purge"), Summary("Deletes messages from said channel (Provide a number on how much messages to delete)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeCmd (int input) 
        {

            if (input > 100) {
                await ReplyAsync("You can not delete more than 100 messages at once.");
                return;
            }
            if (input <= 0) {
                await ReplyAsync("What? No.");
                return;
            }

            try 
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(input).FlattenAsync();

                ITextChannel channel = (ITextChannel)Context.Channel;
                ChannelPermissions channelPermissions = Context.Guild.CurrentUser.GetPermissions(channel);

                if (channelPermissions.ManageMessages) {
                    await channel.DeleteMessagesAsync(messages);
                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Purge", "Successfully deleted " + input + " messages! :ok_hand:"));
                } else {
                    await ReplyAsync("", embed: _lib.CreateEmbedWithError("Purge Error", "I don't seem to have permissions to delete messages.\nTo Delete messages, i must have the **Manage Messages** permission."));
                }

            } catch (Exception e)
            {
                Embed err = _lib.CreateEmbedWithError("Purge Error", $"**Error**: {e.Message}");
                await ReplyAsync("", embed: err);
            }
        }

        [Command("setprefix")]
        [Name("setprefix"), Summary("Sets the prefix used for the server.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefixCmd([Remainder]string newPrefix)
        {
            DatabaseConnection connection = new DatabaseConnection("EdgyBot.db");
            await connection.ConnectAsync();

            if (await connection.OpenConnection())
            {
                try
                {
                    Guild guild = new Guild(Context.Guild.Id);
                    await guild.ChangePrefix(newPrefix);

                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Utility Commands", $"Guild Prefix set to ``{newPrefix}``"));
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not change the server prefix. Error: " + e.Message);
                }

                return;
            }

            await ReplyAsync("Could not open a connection to the Database.");
        }

        [Command("setstatus")][RequireOwner]
        public async Task SetStatusCmd([Remainder]string input = null)
        {
            if (input == "default")
            {
                await Context.Client.SetGameAsync("e!help | EdgyBot for " + Context.Client.Guilds.Count + " servers!");
                await ReplyAsync("Changed Status. **Custom Param: " + input + "**");

                return;
            }
            await Context.Client.SetGameAsync(input);
            await ReplyAsync("Changed Status.");
        }
        [Command("kick")][Name("kick")][Summary("Kicks a user from the guild")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickCmd (IGuildUser usr, [Remainder]string reason = null)
        {
            Embed e = null;
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    await usr.KickAsync();
                } else
                {
                    await usr.KickAsync(reason);
                }
                e = _lib.CreateEmbedWithText("EdgyBot Administrative Commands", "Kicked user " + usr.Username + " for reason " + reason);
            } catch
            {
                e = _lib.CreateEmbedWithError("EdgyBot Administrative Commands Error", ":exclamation: *Could not kick this user.*");
            }
            await ReplyAsync("", embed: e);
        }

        [Command("ban")][Name("ban")][Summary("Bans a user from the Guild. Optionally, You can provide a reason.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanCmd (IGuildUser usr, [Remainder]string reason = null)
        {
            bool hasReason = true;
            if (reason == null) hasReason = false;
            Embed e = null;
            try
            {
                if (hasReason)
                {
                    await usr.Guild.AddBanAsync(usr, reason: reason);
                } else await usr.Guild.AddBanAsync(usr);
                e = _lib.CreateEmbedWithText("EdgyBot Administrative Commands", $"Successfully banned user {usr.Username}!");
            } catch
            {
                e = _lib.CreateEmbedWithError("EdgyBot Administrative Commands Error", ":exclamation: *Could not ban this user.*");
            }
            await ReplyAsync("", embed: e);
        }
    }
}
