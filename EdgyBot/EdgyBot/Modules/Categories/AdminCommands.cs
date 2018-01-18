using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EdgyBot.Modules.Categories
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        private readonly Database database = new Database();
        private readonly LibEdgyBot _libEdgyBot = new LibEdgyBot();

        [Command("execquery")]
        public async Task ExecuteQuery([Remainder]string queryInput)
        {
            if (Context.User.Id == _libEdgyBot.getOwnerID()) database.ExecuteQuery(queryInput);
            await ReplyAsync("INJECTED.");
        }
    }
}
