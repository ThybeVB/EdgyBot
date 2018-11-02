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
        public static Credentials Credentials;
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 25,
            TotalShards = 1
        });

        public async Task StartAsync ()
        {
            Credentials = new CredentialsManager().Read();
            Handler.EventHandler handler = new Handler.EventHandler(Client, new Victoria.Lavalink());
            await new CommandHandler().InitializeAsync(Client, handler.GetLavaManager());
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
