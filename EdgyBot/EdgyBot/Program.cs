using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class Program
    {
        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        public CommandHandler _handler;

        public async Task StartAsync ()
        {
            _client = new DiscordSocketClient();
            Console.WriteLine("Starting Client...");
            new CommandHandler();
            _client = new DiscordSocketClient();
            Console.WriteLine("Connecting to EdgyBot...");
            _client.Ready += Ready;
            string _token = "";
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            Console.WriteLine("Connected!");
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
        public async Task Ready()
        {
            await _client.SetGameAsync("indev");
        }
    }
}
