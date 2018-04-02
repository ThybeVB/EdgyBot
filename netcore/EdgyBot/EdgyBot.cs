using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace EdgyBot
{
    public class EdgyBot
    {
        private static void Main()
            => new EdgyBot().MainAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
        //private readonly LibEdgyBot _lib = new LibEdgyBot();

        private async Task MainAsync ()
        {
            //new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, /*LoginInfo*/"");
            await Client.StartAsync();
            //await new CommandHandler().InitializeAsync(Client);
            await Task.Delay(-1);
        }
    }
}
