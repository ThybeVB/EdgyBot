using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using EdgyCore.Services;
using EdgyBot.Modules;
using EdgyCore.Lib;

namespace EdgyCore.Handler
{
    public class CommandHandler
    {
        private string _prefix;

        private DiscordShardedClient _client;
        private IServiceProvider _service;
        private CommandService commandService;

        private readonly LibEdgyCore _coreLib = new LibEdgyCore();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordShardedClient client)
        {
            _client = client;

            _service = ConfigureServices();
            _prefix = _coreLib.GetPrefix();
            commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);

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
            if (msg == null || msg.Author.IsBot) return;
            ShardedCommandContext context = new ShardedCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                IResult result = await commandService.ExecuteAsync(context, argPos, _service);             

                if (!result.IsSuccess)
                {
                    if (result.Error != CommandError.UnknownCommand)
                    {
                        if (result.Error != CommandError.Exception)
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
                        } else
                        {
                            await context.Channel.SendMessageAsync("**Internal Error**: Either the bot is still starting, or there is an actual problem. Please use e!bugreport if you think this is an actual problem.");
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