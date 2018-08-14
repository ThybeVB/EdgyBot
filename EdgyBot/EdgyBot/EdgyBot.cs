using System;
using System.Threading.Tasks;
using EdgyCore.Models;
using EdgyCore.Handler;
using EdgyCore.Lib;
using Discord;
using Discord.WebSocket;
using SharpLink;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public static Credentials Credentials;
        private readonly LibEdgyCore _core = new LibEdgyCore();
        private LavalinkManager _manager;

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            MessageCacheSize = 100,
            TotalShards = 2
        });

        private async Task StartAsync ()
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

            Credentials = _core.GetCredentials();
            Handler.EventHandler handler = new Handler.EventHandler(Client, _manager);
            await new CommandHandler().InitializeAsync(Client, handler.GetManager());
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}