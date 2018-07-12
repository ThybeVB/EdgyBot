using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using System.Linq;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Addons.Interactive;
using EdgyCore.Services;
using EdgyBot.Modules;
using EdgyCore.Lib;
using EdgyBot.Database;

namespace EdgyCore.Handler
{
    public class CommandHandler
    {
        private string _prefix = "e!";

        private DiscordShardedClient _client;
        private IServiceProvider _service;
        private CommandService commandService;
        private DatabaseConnection databaseConnection = new DatabaseConnection();

        private readonly LibEdgyCore _coreLib = new LibEdgyCore();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordShardedClient client)
        {
            _client = client;
            _service = ConfigureServices();
            _prefix = _coreLib.GetPrefix();

            await databaseConnection.ConnectAsync();
            await databaseConnection.OpenConnection();

            commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);

            new HelpCommand(commandService);

            _client.MessageReceived += HandleCommandAsync;
        }

        private IServiceProvider ConfigureServices ()
        {
            return new ServiceCollection()
                .AddSingleton(new AudioService())
                .AddSingleton(new InteractiveService(_client))
                .BuildServiceProvider();
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null || msg.Author.IsBot) return;

            ShardedCommandContext context = new ShardedCommandContext(_client, msg);

            Guild guild = new Guild(context.Guild.Id);
            _prefix = await guild.GetPrefix(context.Guild.Id);

            int argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                string rawCmd = msg.Content.Substring(_prefix.ToCharArray().Count());
                if (await guild.CommandDisabled(context.Guild.Id, rawCmd))
                    return;

                IResult result = await commandService.ExecuteAsync(context, argPos, _service);             

                if (!result.IsSuccess)
                {
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            await context.Channel.SendMessageAsync($"You gave wrong or no input in this command. Check the **{_prefix}help** command for info.");
                            break;
                        case CommandError.UnknownCommand:
                            await _lib.EdgyLog(LogSeverity.Verbose, $"Unknown Command Executed, '{msg.Content}'");
                            break;
                        case CommandError.Exception:
                            await context.Channel.SendMessageAsync("**Internal Error**: Please use e!bugreport if you think this is an actual problem.");
                            break;
                        case CommandError.UnmetPrecondition:
                            await _lib.EdgyLog(LogSeverity.Warning, "USER ENCOUNTERED AN ERROR: " + result.ErrorReason);
                             await context.Channel.SendMessageAsync(result.ErrorReason);
                            break;
                        case CommandError.ParseFailed:
                            await _lib.EdgyLog(LogSeverity.Warning, "USER ENCOUNTERED AN ERROR: " + result.ErrorReason);
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                            break;
                    }
                }
            }
        }
    }
}