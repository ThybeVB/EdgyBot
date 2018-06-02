using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Handler;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credientals;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        });


        private async Task StartAsync ()
        {
            Credientals = _lib.GetCredientals();
            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credientals.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);
            await Task.Delay(-1);
        }
    }
}