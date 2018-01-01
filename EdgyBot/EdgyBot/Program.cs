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

        public DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        });
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly libEdgyBot _lib = new libEdgyBot();

        public async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Client.Log += _lib.Log;
            Client.Ready += Ready;
            Client.UserLeft += UserLeft;
            Client.JoinedGuild += Client_JoinedGuild;
            Client.Connected += Client_Connected;
            
            await Client.LoginAsync(TokenType.Bot, _lib.getToken());
            await Client.StartAsync();           
            await _handler.InitializeAsync(Client);

            await Task.Delay(-1);
        }

        private async Task Client_Connected()
        {
            await _lib.Log(new LogMessage(LogSeverity.Verbose, "EDGYBOT", "CONNECTED"));
        }

        private static async Task Client_JoinedGuild(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("SH*T THANKS FOR INVITING ME M8'S, TO SEE ME COMMANDS, USE **e!help**.");
        }
        public async Task Ready()
        {
            await Client.SetGameAsync("e!help | EdgyBot for " + Client.Guilds.Count + " servers!");
        }      
        private async Task UserLeft(SocketGuildUser user)
        {
            var dm = await user.GetOrCreateDMChannelAsync();
            var e = _lib.createEmbedWithText("EdgyBot", "We hope you enjoyed your stay, " + user.Username + "!");
            await ReplyAsync("", embed: e);
        }
    }
}


