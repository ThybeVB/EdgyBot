using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace EdgyBot
{
    public class EdgyBot
    {
        private static void Main()
            => new EdgyBot().MainAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task MainAsync ()
        {
            IWebHost host = WebHost.CreateDefaultBuilder().UseStartup<ASPStartup>().Build();
            new EventHandler(Client);
            await Client.LoginAsync(TokenType.Bot, _lib.GetToken());
            await Client.StartAsync();
            await new CommandHandler().InitializeAsync(Client);
            host.Run();
            await Task.Delay(-1);
        }
    }
    internal class ASPStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*
             * The reason i am using a configurator, is a bug in ASP.NET. For some reason,
             * if you don't use one, you will get errors upon running.
             */
        }
    }
}
