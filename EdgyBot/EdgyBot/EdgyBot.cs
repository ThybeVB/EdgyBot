using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Common;
using EdgyCore.Handler;
using EdgyCore.Lib;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credientals;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        });

        private async Task StartAsync ()
        {
            Credientals = _core.GetCredientals();
            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credientals.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}