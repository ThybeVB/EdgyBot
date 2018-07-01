using System;
using System.Threading.Tasks;
using Discord.Commands;
using EdgyBot.Database;

namespace EdgyBot.Modules.Categories
{
    public class OwnerCommands : ModuleBase<ShardedCommandContext>
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

        [Command("execquery")]
        [RequireOwner]
        public async Task ExecQueryCmd ()
        {
            DatabaseConnection connection = new DatabaseConnection("EdgyBot.db");
            await connection.ConnectAsync();
            if (await connection.OpenConnection())
            {
                return;
            }

            await ReplyAsync("Could not open a conenction to the Database.");
        }
    }
}
