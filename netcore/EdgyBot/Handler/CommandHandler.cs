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
        private Database _database = new Database();

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
                if (msg.Author.IsBot) return;

                IResult result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                {
                    if (result.Error != CommandError.UnknownCommand)
                    {
                        if (result.Error != CommandError.BadArgCount)
                        {
                            await _lib.EdgyLog(LogSeverity.Critical, "USER ENCOUNTERED AN ERROR: " + result.ErrorReason);
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                        }
                        else
                        {
                            await context.Channel.SendMessageAsync($"You gave wrong input in this command. Check the **{_lib.GetPrefix()}help** command for info.");
                        }
                    }
                    else
                    {
                        await context.Channel.SendMessageAsync($"This command does not exist. Try **{_lib.GetPrefix()}help**");
                    }
                }
                else
                {
                    if (!context.User.IsBot)
                    {
                        //_database.AddExperience(context.User.Id);
                    }
                }
            }
        }
    }
}