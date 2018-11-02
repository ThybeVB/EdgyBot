using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using System.Linq;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Addons.Interactive;
using EdgyBot.Modules;
using EdgyBot.Core.Lib;
using EdgyBot.Database;
using Victoria;
using EdgyBot.Services;

namespace EdgyBot.Core.Handler
{
    public class CommandHandler
    {
        private string _prefix = "e!";

        private DiscordShardedClient _client;
        private IServiceProvider _service;
        private CommandService commandService;
        private DatabaseConnection databaseConnection = new DatabaseConnection();

        private Lavalink _manager;

        public static int CommandsRan = 0;

        private readonly LibEdgyCore _coreLib = new LibEdgyCore();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task InitializeAsync(DiscordShardedClient client, Lavalink lavalink)
        {
            _client = client;
            _manager = lavalink;

            _service = ConfigureServices();
            _prefix = _coreLib.GetPrefix();

            await _lib.EdgyLog(LogSeverity.Info, $"EdgyBot v{Assembly.GetExecutingAssembly().GetName().Version}");
            await _lib.EdgyLog(LogSeverity.Info, $"Loading EdgyBot with {_client.Shards.Count} Shards");

            _manager.Log += _lib.LavalinkLog;

            await databaseConnection.ConnectAsync();
            databaseConnection.OpenConnection();

            commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true
            });
            await commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _service);

            new HelpCommand(commandService);

            _client.MessageReceived += HandleCommandAsync;
        }

        private IServiceProvider ConfigureServices ()
        {
            return new ServiceCollection()
                .AddSingleton(new InteractiveService(_client))
                .AddSingleton(_manager)
                .AddSingleton(new AudioService(_manager))
                .BuildServiceProvider();
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null || msg.Author.IsBot) return;

            ShardedCommandContext context = new ShardedCommandContext(_client, msg);

            Guild guild = new Guild(context.Guild.Id);
            _prefix = guild.GetPrefix();

            int argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                string rawCmd = msg.Content.Substring(_prefix.ToCharArray().Count());
                string firstArg = rawCmd.Split(' ')[0];
                if (guild.CommandDisabled(firstArg))
                    return;

                CommandsRan++;
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