using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EdgyBot.Modules.Categories
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Database database = new Database();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("execquery")]
        public async Task ExecuteQuery([Remainder]string queryInput)
        {
            if (Context.User.Id == _lib.GetOwnerID())
            {
                database.ExecuteQuery(queryInput);
                Embed e = _lib.CreateEmbedWithText("Success", "Code " + queryInput + " has been executed.");
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }        
        }
        [Command("isblacklisted")]
        [Alias("blacklisted")]
        public async Task IsBlackListedCmd (ulong serverID)
        {
            bool isBlackListed = database.IsServerBlacklisted(serverID);
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
                if (!database.IsServerBlacklisted(guild.Id))
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
        [Command("stopannounce")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task StopAnnounceCmd()
        {
            ulong serverID = Context.Guild.Id;
            database.BlacklistServer(serverID);
            Embed e = _lib.CreateEmbedWithText("Announcement Unsub", Context.Guild.Name + " has been excluded from recieving announcements.");

            await ReplyAsync("", embed: e);
        }
    }
}
