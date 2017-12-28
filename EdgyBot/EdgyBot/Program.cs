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

        public DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        });
        public CommandHandler _handler = new CommandHandler();
        private LoginInfo loginInfo = new LoginInfo();
        private libEdgyBot lib = new libEdgyBot();

        public async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            _client.Log += lib.Log;
            _client.Ready += Ready;
            _client.UserLeft += UserLeft;
            
            await _client.LoginAsync(TokenType.Bot, lib.getToken());
            await _client.StartAsync();           
            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
        public async Task Ready()
        {
            int guildNum = _client.Guilds.Count;
            await _client.SetGameAsync("e!help | EdgyBot for " + guildNum + " servers!");
        }      
        private async Task UserLeft(SocketGuildUser user)
        {
            IDMChannel dm = await user.GetOrCreateDMChannelAsync();
            Embed e = lib.createEmbedWithText("EdgyBot", "We hope you enjoyed your stay, " + user.Username + "!");
            await ReplyAsync("", embed: e);
        }
    }
}
