using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace EdgyBot.Modules.Categories
{
    public class OwnerCommands : ModuleBase<SocketCommandContext>
    {
        [Command("setstatus"), RequireOwner]
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
    }
}
