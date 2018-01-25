using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using EdgyBot.Modules;

namespace EdgyBot
{
    /*
        EDGYBOT /// 2017-2018
        DISCLAIMER
        IF YOU WANT TO MODIFY OR RUN THIS BOT, PLEASE CREATE A CLASS CALLED LoginInfo.cs WITH THE FOLLOWING VARIABLES
        *************
        string token
        string prefix 
        string invLink
        string GJP
        string accID
        ulong ownerID
        **************
        GJP AND ACCID ARE GEOMETRY DASH VARIABLES, IF YOU DO NOT HAVE THEM SIMPLY LEAVE THEM EMPTY
     */
    public class EdgyBot
    {
        private static void Main(string[] args)
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose, DefaultRetryMode = RetryMode.RetryRatelimit});
        private readonly CommandHandler  _handler = new CommandHandler();
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private EventHandler _ehandler;

        private async Task StartAsync ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            _ehandler = new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, _lib.GetToken());
            await Client.StartAsync();          
            await _handler.InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}


