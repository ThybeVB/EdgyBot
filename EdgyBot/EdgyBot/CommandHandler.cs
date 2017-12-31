using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        public CommandService _service;
        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommandAsync;
        }
        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix("e!", ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                {
                    if (result.ToString() == "The server responded with error 50007: Cannot send messages to this user")
                    {
                        await context.Channel.SendMessageAsync("**ERROR:** Could not send messages to this user. Either the user does not allow private messages from unknown's, or it is a bot.");
                        return;
                    }
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
