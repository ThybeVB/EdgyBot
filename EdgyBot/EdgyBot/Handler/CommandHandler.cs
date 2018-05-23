using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using EdgyCore.Services;
using EdgyCore.Modules;

namespace EdgyCore.Handler
{
    public class CommandHandler
    {
        private string _prefix;

        private DiscordSocketClient _client;
        private IServiceProvider _service;
        private CommandService commandService;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;

            _service = ConfigureServices();
            _prefix = _lib.GetPrefix();

            commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            new HelpCommand(commandService);

            _client.MessageReceived += HandleCommandAsync;
        }

        private IServiceProvider ConfigureServices ()
        {
            return new ServiceCollection()
                .AddSingleton(new AudioService())
                .BuildServiceProvider();
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null) return;
            SocketCommandContext context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                if (msg.Author.IsBot) return;
                if (msg.Content == _prefix) return;

                IResult result = await commandService.ExecuteAsync(context, argPos, _service);             

                if (!result.IsSuccess)
                {
                    if (result.Error != CommandError.UnknownCommand)
                    {
                        if (result.Error != CommandError.BadArgCount)
                        {
                            await _lib.EdgyLog(LogSeverity.Warning, "USER ENCOUNTERED AN ERROR: " + result.ErrorReason);
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                        }
                        else
                        {
                            await context.Channel.SendMessageAsync($"You gave wrong or no input in this command. Check the **{_prefix}help** command for info.");
                        }
                    }
                    else
                    {
                        await _lib.EdgyLog(LogSeverity.Verbose, $"Unknown Command Executed, '{msg.Content}'");
                    }
                }
            }
        }
    }
}