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

        public readonly DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose, DefaultRetryMode = RetryMode.RetryRatelimit});
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private EventHandler _ehandler;

        public async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _ehandler = new EventHandler(client);
            await client.LoginAsync(TokenType.Bot, _lib.getToken());
            await client.StartAsync();          
            await _handler.InitializeAsync(client);

            await Task.Delay(-1);
        }
    }
}


