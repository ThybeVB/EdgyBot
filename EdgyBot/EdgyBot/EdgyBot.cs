using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class EdgyBot
    {
        /*
         * EdgyBot 2018
         * If you are trying to run this bot, make sure to read the README to see how you can set up the bot.
        */
        private static void Main () 
            => new EdgyBot().MainAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose});
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        private async Task MainAsync ()
        {
            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, _lib.GetToken());
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);
            await Task.Delay(-1);
        }
    }
}