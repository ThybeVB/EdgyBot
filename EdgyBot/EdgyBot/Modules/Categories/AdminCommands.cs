using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyCore.Handler.Pinger;

namespace EdgyCore.Modules.Categories
{
    [Name("Admin Commands"), Summary("Administrative Commands")]
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("sendcountbfd", RunMode = RunMode.Async), RequireOwner]
        public async Task SendJson()
        {
            await new BotsForDiscordPinger().SendServerCountAsync();
            await ReplyAsync("OK");
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
