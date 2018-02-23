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
                try { _database.ExecuteQuery(queryInput); } catch { await ReplyAsync("Error executing query."); return; }
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
        [Command("announce", RunMode = RunMode.Async)]
        public async Task AnnounceCmd([Remainder]string msg)
        {
            if (Context.User.Id != _lib.GetOwnerID())
            {
                await ReplyAsync("Permission Denied.");
                return;
            }
            await _lib.EdgyLog(LogSeverity.Info, "Sending announcement " + msg);
            int sentCount = 0;
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
                        sentCount++;
                    }
                    catch
                    {
                        await _lib.EdgyLog(LogSeverity.Error, "Could not send announcement to " + guild.Name);
                    }               
                }
            }
            await ReplyAsync("Sent message to " + sentCount + " servers.");
        }
        [Command("stopannounce")][Name("stopannounce")][Summary("Stops receiving announcements from EdgyBot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task StopAnnounceCmd()
        {
            _database.BlacklistServer(Context.Guild.Id);
            Embed e = _lib.CreateEmbedWithText("Announcement Unsub", Context.Guild.Name + " has been excluded from recieving announcements.");

            await ReplyAsync("", embed: e);
        }
        [Command("enableannounce")][Name("enableannounce")][Summary("Start recieving announcements again.")]
        public async Task EnableAnnounceCmd ()
        {
            _database.DeleteFromBlacklisted(Context.Guild.Id);
            Embed e = _lib.CreateEmbedWithText("Announcement Enable", Context.Guild.Name + " has started recieving announcements again.");
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
