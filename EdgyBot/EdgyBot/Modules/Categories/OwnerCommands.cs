using System;
using System.Threading.Tasks;
using Discord.Commands;
using EdgyBot.Database;
using EdgyCore;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Discord;
using System.Text;

namespace EdgyBot.Modules.Categories
{
    public class OwnerCommands : InteractiveBase<ShardedCommandContext>
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        [Command("setstatus"), RequireOwner]
        public async Task SetStatusCmd([Remainder]string input = null)
        {
            if (input == "default") {
                await Context.Client.SetGameAsync("e!help | EdgyBot for " + Context.Client.Guilds.Count + " servers!");
                await ReplyAsync("Changed Status. **Custom Param: " + input + "**");

                return;
            }

            await Context.Client.SetGameAsync(input);
            await ReplyAsync("Changed Status.");
        }

        [Command("listservers", RunMode = RunMode.Async)]
        public async Task ListServersCmd ()
        {
            await ReplyAsync("ok my dude");

            StringBuilder sb = new StringBuilder();
            foreach (IGuild guild in Context.Client.Guilds)
            {
                if (guild == null)
                    continue;

                sb.Append(guild.Name + ",");
            }
            string[] pages = (sb.ToString()).Split(',');

            await PagedReplyAsync(pages);
        }

        [Command("execquery")]
        [RequireOwner]
        public async Task ExecQueryCmd ([Remainder]string query)
        {
            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
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
