using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Discord;
using Discord.WebSocket;

namespace EdgyCore
{
    public class EdgyBot
    {
        private static void Main ()
            => new EdgyBot().MainAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        private async Task MainAsync ()
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
}