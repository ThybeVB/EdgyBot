using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using EdgyCore.Modules;
using EdgyCore.Services;

namespace EdgyCore.Handler
{
    public class CommandHandler
    {
        private string _prefix;

        private DiscordSocketClient _client;
        private CommandService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;

            _prefix = _lib.GetPrefix();
            _service = new CommandService();

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());



            new HelpCommand(_service);

            _client.MessageReceived += HandleCommandAsync;
        }

        //private IServiceProvider ConfigureServices ()
        //{
        //    return new ServiceCollection()
        //        .AddSingleton<AudioService>()
        //        .BuildServiceProvider();
        //    
        //}

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null) return;
            SocketCommandContext context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos))
            {
                if (msg.Author.IsBot) return;
                if (msg.Content == _prefix) return;

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
                            await context.Channel.SendMessageAsync($"You gave wrong or no input in this command. Check the **{_prefix}help** command for info.");
                        }
                    }
                    else
                    {
                        await _lib.EdgyLog(LogSeverity.Verbose, $"Unknownd Command Executed, '{msg.Content}'");
                        //await context.Message.AddReactionAsync(new Emoji("❓"));
                    }
                }
            }
        }
    }
}