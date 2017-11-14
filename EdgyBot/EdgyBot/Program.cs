using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;

namespace EdgyBot
{
    public class Program : ModuleBase<SocketCommandContext>
    {
        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        public CommandHandler _handler;

        public async Task StartAsync ()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });            

            Console.ForegroundColor = ConsoleColor.Green;
            _client.Log += Log;
            _client.Ready += Ready;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;
            string _token = "";
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
        public async Task Ready()
        {
            await _client.SetGameAsync("$");
        }
        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
        private async Task UserJoined(SocketGuildUser user)
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();

            string guildname = Context.Guild.Name;
            await Context.Channel.SendMessageAsync("Welcome to " + guildname + ", " + Context.User.Username + "!");
            await dm.SendMessageAsync("Welcome to " + guildname + ", " + Context.User.Username + "!");
        }
        private async Task UserLeft(SocketGuildUser user)
        {
            await Context.Channel.SendMessageAsync("Sad to see you leave, " + Context.User.Username);
        }
    }
}
