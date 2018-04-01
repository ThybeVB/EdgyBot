using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EdgyBot.Modules.Categories
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Database _database = new Database();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("execquery")][RequireOwner]
        public async Task ExecuteQuery([Remainder]string queryInput)
        {
            if (Context.User.Id == _lib.GetOwnerID())
            {
                try { _database.ExecuteQuery(queryInput); } catch
                {
                    await ReplyAsync("Error executing query.");
                    return;
                }
                Embed e = _lib.CreateEmbedWithText("Success", "Code " + queryInput + " has been executed.");
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }        
        }
        [Command("isblacklisted")]
        [Alias("blacklisted")]
        [Name("blacklisted")]
        [Summary("Checks if a server is blacklisted.")]
        public async Task IsBlackListedCmd (ulong serverID)
        {
            if (Context.Client.GetGuild(serverID) != null)
            {
                bool isBlackListed = _database.IsServerBlacklisted(serverID);
                if (isBlackListed)
                {
                    await ReplyAsync("This server is blacklisted.");
                }
                else
                {
                    await ReplyAsync("This server is not blacklisted.");
                }
            } else
            {
                await ReplyAsync("I'm not in that server! :cry:");
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
