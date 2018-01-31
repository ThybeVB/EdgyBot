using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using EdgyBot.Modules;

namespace EdgyBot
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            new HelpCommand(_service);

            _client.MessageReceived += HandleCommandAsync;
        }
        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null) return;
            SocketCommandContext context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(_lib.GetPrefix(), ref argPos))
            {
                IResult result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                {
                    await _lib.EdgyLog(LogSeverity.Critical, "USER ENCOUNTERED AN ERROR: " + result.ErrorReason);
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
