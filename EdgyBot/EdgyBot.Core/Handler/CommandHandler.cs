using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using Discord.WebSocket;
using Discord.Commands;
using EdgyBot.Modules;
using EdgyBot.Core.Lib;
using EdgyBot.Database;

namespace EdgyBot.Core.Handler
{
    public class CommandHandler
    {
        private string _prefix = "e!";

        private DiscordShardedClient _client;
        private DatabaseConnection databaseConnection = new DatabaseConnection();

        public static int CommandsRan = 0;

        private readonly LibEdgyCore _coreLib = new LibEdgyCore();
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private IServiceProvider _provider;
        private CommandService commandService;

        public CommandHandler (CommandService service)
        {
            commandService = service;
        }

        public async Task InitializeAsync(DiscordShardedClient client, IServiceProvider provider)
        {
            _client = client;
            _provider = provider;
            _prefix = _coreLib.GetPrefix();

            await _lib.EdgyLog(LogSeverity.Info, $"EdgyBot v{Assembly.GetExecutingAssembly().GetName().Version}");
            await _lib.EdgyLog(LogSeverity.Info, $"Loading EdgyBot with {_client.Shards.Count} Shards");

            await databaseConnection.ConnectAsync();
            databaseConnection.OpenConnection();

            commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true
            });
            await commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), provider);

            new HelpCommand(commandService);

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = (SocketUserMessage)s;
            if (msg == null || msg.Author.IsBot) return;

            EbShardContext context = new EbShardContext(_client, msg);

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
                IResult result = await commandService.ExecuteAsync(context, argPos, _provider);             

                if (!result.IsSuccess)
                {
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            await context.Channel.SendMessageAsync($"{context.Language["core"]["badArgCount1"]}{_prefix}{context.Language["core"]["badArgCount2"]}");
                            break;
                        case CommandError.UnknownCommand:
                            await _lib.EdgyLog(LogSeverity.Verbose, $"Unknown Command Executed, '{msg.Content}'");
                            break;
                        case CommandError.Exception:
                            await context.Channel.SendMessageAsync((string)context.Language["core"]["exception"]);
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