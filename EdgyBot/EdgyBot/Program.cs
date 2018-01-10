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

        private readonly DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose, DefaultRetryMode = RetryMode.RetryRatelimit});
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private EventHandler _ehandler;

        public async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _ehandler = new EventHandler(_client);
            await _client.LoginAsync(TokenType.Bot, _lib.getToken());
            await _client.StartAsync();          
            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
    }
}


