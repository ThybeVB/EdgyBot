using System;
using System.Threading.Tasks;
using EdgyBot.Core.Models;
using EdgyBot.Core.Handler;
using EdgyBot.Core.Lib;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using EdgyBot.Services;
using Discord.Addons.Interactive;
using Victoria;
using HyperEx;
using System.Reflection;
using Discord.Commands;

namespace EdgyBot.Core
{
    public class Bot
    {
        public static Credentials Credentials;
        private readonly LibEdgyCore _core = new LibEdgyCore();
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        public readonly DiscordShardedClient Client = new DiscordShardedClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 10,
            TotalShards = 1
        });

        public async Task StartAsync ()
        {
            Credentials = new CredentialsManager().Read();

            var services = new ServiceCollection();
            CommandService service = ConfigureServices(services, Client);

            var provider = services.BuildServiceProvider();
            provider.InjectProperties(_assembly, typeof(InjectAttribute));

            await new CommandHandler(service).InitializeAsync(Client, provider);
            await Client.LoginAsync(TokenType.Bot, Credentials.token);
            await Client.StartAsync();
            
            await Task.Delay(-1);
        }

        private CommandService ConfigureServices(IServiceCollection services, DiscordShardedClient client)
        {
            CommandService service = new CommandService();

            services.AddSingleton(new InteractiveService(client));
            services.AddSingleton<Lavalink>();
            services.AddSingleton(service);
            services.AddSingleton(client);
            services.AddSingleton<AudioService>();
            services.AddSingleton<LocalizationService>();
            services.RegisterSubclasses(_assembly, typeof(BaseService), true);

            return service;
        }
    }
}
