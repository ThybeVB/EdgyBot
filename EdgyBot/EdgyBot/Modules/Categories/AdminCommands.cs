using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EdgyBot.Modules.Categories
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Database _database = new Database();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("execquery")]
        public async Task ExecuteQuery([Remainder]string queryInput)
        {
            if (Context.User.Id == _lib.GetOwnerID())
            {
                _database.ExecuteQuery(queryInput);
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
            bool isBlackListed = _database.IsServerBlacklisted(serverID);
            if (isBlackListed)
            {
                await ReplyAsync("This server is blacklisted.");
            } else
            {
                await ReplyAsync("This server is not blacklisted.");
            } 
        }
        [Command("announce")]
        public async Task AnnounceCmd([Remainder]string msg)
        {
            if (Context.User.Id != _lib.GetOwnerID())
            {
                await ReplyAsync("Permission Denied.");
                return;
            }
            await _lib.EdgyLog(LogSeverity.Info, "Sending announcement " + msg);
            foreach (IGuild guild in Context.Client.Guilds)
            {
                if (guild == null) continue;
                if (!_database.IsServerBlacklisted(guild.Id))
                {
                    ITextChannel channel =  await guild.GetDefaultChannelAsync();
                    if (channel == null) continue;
                    
                    Embed e = _lib.CreateAnnouncementEmbed(msg, true);
                    try
                    {
                        await channel.SendMessageAsync("", embed: e);
                    }
                    catch
                    {
                        await _lib.EdgyLog(LogSeverity.Error, "Could not send announcement to " + guild.Name);
                    }                    
                }
            }
            await ReplyAsync("Sent message to " + Context.Client.Guilds.Count + " servers.");
        }
        [Command("stopannounce")][Name("stopannounce")][Summary("Stops receiving announcements from EdgyBot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task StopAnnounceCmd()
        {
            ulong serverID = Context.Guild.Id;
            _database.BlacklistServer(serverID);
            Embed e = _lib.CreateEmbedWithText("Announcement Unsub", Context.Guild.Name + " has been excluded from recieving announcements.");

            await ReplyAsync("", embed: e);
        }
        [Command("setstatus")]
        public async Task SetStatusCmd([Remainder]string input)
        {
            if (Context.User.Id == _lib.GetOwnerID())
            {
                await Context.Client.SetGameAsync(input);
                await ReplyAsync("Changed Status.");
            }
            else
            {
                await ReplyAsync("No Permissions.");
            }
        }
    }
}
