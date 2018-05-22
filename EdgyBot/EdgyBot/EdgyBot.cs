using System.Threading;
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

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        private async Task StartAsync ()
        {
            Credientals = _lib.GetCredientals();
            if (Credientals == null)
                return;

            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credientals.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);

            await Task.Delay(Timeout.Infinite);
        }
    }
}