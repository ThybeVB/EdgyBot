using System;
using System.Threading.Tasks;
using Discord.Commands;
using EdgyBot.Database;
using EdgyCore;

namespace EdgyBot.Modules.Categories
{
    public class OwnerCommands : ModuleBase<ShardedCommandContext>
    {
        private LibEdgyBot _lib = new LibEdgyBot();

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
        public async Task ExecQueryCmd ([Remainder]string query)
        {
            DatabaseConnection connection = new DatabaseConnection("EdgyBot.db");
            await connection.ConnectAsync();

            if (await connection.OpenConnection())
            {
                try
                {
                    SQLProcessor sql = new SQLProcessor(connection.getConnObj());
                    await sql.ExecuteQueryAsync(query);
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not change the server prefix. Error: " + e.Message);
                }

                return;
            }
        }
    }
}
