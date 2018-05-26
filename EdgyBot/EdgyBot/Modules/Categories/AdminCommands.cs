using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyCore;

namespace EdgyBot.Modules.Categories
{
    [Name("Admin Commands"), Summary("Administrative Commands")]
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("purge", RunMode = RunMode.Async), Name("purge"), Summary("Deletes messages from the channel (Provide a number on how much messages to delete)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeCmd (int input) 
        {
            int original = input;

            if (input > 100) {
                await ReplyAsync("You can't delete more than 100 messages at once.");
                return;
            }
            
            var messages = await Context.Channel.GetMessagesAsync(input).FlattenAsync();
            try 
            {
                var toDelete = await ReplyAsync("Started Purge. This may take a while. Deleting " + input + " messages.");
                foreach (var message in messages) 
                {
                    if (message == null)
                        continue;
                    input--;
                    await Task.Delay(TimeSpan.FromSeconds(2.5));
                    await message.DeleteAsync();
                    await toDelete.ModifyAsync(x => x.Content = "Executing Purge. This may take a while. Messages to Delete: " + input);
                }
                await toDelete.DeleteAsync();
                await ReplyAsync("", embed: _lib.CreateEmbedWithText("Purge", "Successfully deleted " + original + " messages :ok_hand:"));
            } catch 
            {
                Embed err = _lib.CreateEmbedWithError("Purge Error", "Bot does not have permission to delete messages.");
                await ReplyAsync("", embed: err);
            }
        }

        [Command("setstatus")][RequireOwner]
        public async Task SetStatusCmd([Remainder]string input = null)
        {
            if (Context.User.Id == _lib.GetOwnerID())
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
            else
            {
                await ReplyAsync("No Permissions.");
            }
        }
        [Command("lookup"), RequireOwner]
        public async Task LookupCmd (ulong guildID)
        {
            SocketGuild guild;

            try { Context.Client.GetGuild(guildID); } catch
            {
                await ReplyAsync("Could not find Guild " + guildID.ToString());
                return;
            }
            guild = Context.Client.GetGuild(guildID);
            await ReplyAsync(guild.Name);
        }

        /*
         * EdgyBot Administratory Commands
        */
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
       // VV postponed 
       //[Command("bancommand", RunMode = RunMode.Async)][Name("bancommand")][Summary("This will ban a command from your server. Be sure to spell the command right!")]
       //public async Task BlacklistCmd ([Remainder]string commandStr)
       //{
       //    Embed e = null;
       //    try
       //    {
       //        _database.BanCommand(System.Convert.ToString(Context.Guild.Id), commandStr);
       //        e = _lib.CreateEmbedWithText("Success", "This command has been blacklisted!");
       //    } catch
       //    {
       //        e = _lib.CreateEmbedWithError("Error", "Error while Blacklisting this command. Did you spell it correctly?");
       //    }
       //    await ReplyAsync("", embed: e);
       //}
    }
}
