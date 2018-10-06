using System;
using System.Threading.Tasks;
using EdgyBot.Core.Models;
using EdgyBot.Core.Handler;
using EdgyBot.Core.Lib;
using Discord;
using Discord.WebSocket;
using SharpLink;

namespace EdgyBot.Core
{
    public class Bot
    {
        private static void Main ()
            => new Bot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credentials;
        private readonly LibEdgyCore _core = new LibEdgyCore();
        private LavalinkManager _manager;

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 25,
            TotalShards = 2
        });

        public async Task StartAsync ()
        {
            _manager = new LavalinkManager(Client, new LavalinkManagerConfig
            {
                RESTHost = "localhost",
                RESTPort = 2333,
                WebSocketHost = "localhost",
                WebSocketPort = 1337,
                Authorization = Environment.GetEnvironmentVariable("EdgyBot_LavaAuth", EnvironmentVariableTarget.User),
                TotalShards = 2,
                LogSeverity = LogSeverity.Verbose
            });

            Credentials = new CredentialsManager().Read();
            Handler.EventHandler handler = new Handler.EventHandler(Client, _manager);
            await new CommandHandler().InitializeAsync(Client, handler.GetManager());
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}