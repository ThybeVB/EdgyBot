using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EdgyBot.Modules.Categories
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Database database = new Database();
        private readonly LibEdgyBot _libEdgyBot = new LibEdgyBot();

        [Command("execquery")]
        public async Task ExecuteQuery([Remainder]string queryInput)
        {
            if (Context.User.Id == _libEdgyBot.getOwnerID())
            {
                database.ExecuteQuery(queryInput);
                Embed e = _libEdgyBot.createEmbedWithText("Success", "Code " + queryInput + " has been executed.");
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }        
        }
        [Command("stopannounce")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task StopAnnounceCmd()
        {
            ulong serverID = Context.Guild.Id;
            database.BlacklistServer(serverID);
            Embed e = _libEdgyBot.createEmbedWithText("Announcement Unsub", Context.Guild.Name + " has been excluded from recieving announcements.");

            await ReplyAsync("", embed: e);
        }
    }
}
