using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgySharp
{
    public class EdgyBot
    {
        private static void Main () => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        public readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig{LogLevel = LogSeverity.Verbose});

        private async Task StartAsync()
        {
            Client.Log += Log;
            await Client.LoginAsync(TokenType.Bot, "");
            await Client.StartAsync();

            await Task.Delay(-1);
        }
        private Task Log (LogMessage lMsg)
        {
            Console.WriteLine(lMsg.ToString());
            return Task.CompletedTask;
        }
    }  
}
