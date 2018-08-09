using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Models;
using EdgyCore.Handler;
using EdgyCore.Lib;

namespace EdgyCore
{
    public class EdgyBot
    {
        //if you want to use the source code of edgybot you have to pay 30$ for the website and an additional DLC for the bot at 50$ without updates n stuff,
        //credit to equalizer for not giving me permission to use website sourcews
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credentials;
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 100,
            TotalShards = 2
        });

        private async Task StartAsync ()
        {
            Credentials = _core.GetCredentials();
            EventHandler handler = new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);

            await Task.Delay(-1);
        }
    }
}