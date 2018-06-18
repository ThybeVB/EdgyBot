using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyCore;

namespace EdgyBot.Modules.Categories
{
    [Name("Admin Commands"), Summary("Administrative Commands")]
    public class AdminCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("purge", RunMode = RunMode.Async), Name("purge"), Summary("Deletes messages from said channel (Provide a number on how much messages to delete)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeCmd (int input, string args = null) 
        {
            int original = input;

            if (input > 100) {
                await ReplyAsync("You can not delete more than 100 messages at once.");
                return;
            }
            
            try 
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(input).FlattenAsync();

                ITextChannel channel = Context.Channel as ITextChannel;
                IGuildUser b = Context.Client.CurrentUser as IGuildUser;
                ChannelPermissions c = b.GetPermissions(Context.Channel as IGuildChannel);

                if (c.ManageMessages) {
                    await channel.DeleteMessagesAsync(messages);
                    await ReplyAsync("", embed: _lib.CreateEmbedWithText("Purge", "Successfully deleted " + original + " messages :ok_hand:"));
                }
            } catch (Exception e)
            {
                Embed err = _lib.CreateEmbedWithError("Purge Error", $"**Error**: {e.Message}");
                await ReplyAsync("", embed: err);
            }
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
