using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using EdgyBot.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EdgyBot
{
    public class Program : ModuleBase<SocketCommandContext>
    {
        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        public CommandHandler _handler;
        private Settings _settings;

        public async Task StartAsync ()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _settings = new Settings();
            _handler = new CommandHandler();

            Console.ForegroundColor = ConsoleColor.Green;

            _client.Log += Log;
            _client.Ready += Ready;
            _client.UserLeft += UserLeft;
            
            await _client.LoginAsync(TokenType.Bot, _settings.getToken());
            await _client.StartAsync();           
            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
        public async Task Ready()
        {
            int guildNum = _client.Guilds.Count;
            await _client.SetGameAsync("$help | EdgyBot [" + guildNum + "]");
        }
        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
        private async Task UserLeft(SocketGuildUser user)
        {
            await ReplyAsync("Sad to see you leave, " + user.Mention + "!");
        }
    }
}
