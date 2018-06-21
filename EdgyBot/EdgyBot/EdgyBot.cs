using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Common;
using EdgyCore.Handler;
using EdgyCore.Lib;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();


        public static Credentials Credentials;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            TotalShards = 2
        });

        private async Task StartAsync ()
        {
            Credentials = _core.GetCredentials();
            EventHandler handler = new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);

            /* A very hacky and cheap way to do it, i know. I only want to run this function after the bot has loaded all shards, so it isn't broken. */
            await Task.Delay(System.TimeSpan.FromMinutes(1));
            await handler.SetupBot();

            await Task.Delay(-1);
        }
    }
}