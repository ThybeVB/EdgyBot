using Discord.Commands;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class GDCommands : ModuleBase<SocketCommandContext>
    {
        [Command("profile")]
        public async Task ProfileGDCMD ()
        {
            await ReplyAsync("soon");
        }
    }
}
