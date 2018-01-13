using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class Program
    {
        private static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose, DefaultRetryMode = RetryMode.RetryRatelimit});
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private EventHandler _ehandler;

        public async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _ehandler = new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, _lib.getToken());
            await Client.StartAsync();          
            await _handler.InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}


