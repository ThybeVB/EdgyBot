using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Models;
using EdgyCore.Handler;
using EdgyCore.Lib;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credentials;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            TotalShards = 2
        });

        private async Task StartAsync ()
        {
            Credentials = _core.GetCredentials();
            EventHandler handler = new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}