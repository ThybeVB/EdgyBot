using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;

namespace EdgyBot
{
    public class EventHandler : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;

            _client.Log += _lib.Log;
            _client.Ready += Ready;
            _client.UserLeft += UserLeft;
            _client.JoinedGuild += Client_JoinedGuild;
            _client.Connected += Client_Connected;
        }
        public async Task Ready()
        {
            string gameStatus = "e!help | EdgyBot for " + _client.Guilds.Count + " servers!";
            await _client.SetGameAsync(gameStatus);
            await _lib.edgyLog(LogSeverity.Info, "Set game to " + gameStatus);
            await _lib.edgyLog(LogSeverity.Info, "Fully loaded EdgyBot v1.0.");
        }
        private async Task Client_Connected()
        {
            await _lib.Log(new LogMessage(LogSeverity.Verbose, "EdgyBot", "Connecting to servers..."));
        }

        private static async Task Client_JoinedGuild(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("SH*T THANKS FOR INVITING ME M8'S, TO SEE ME COMMANDS, USE **e!help**.");
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            var dm = await user.GetOrCreateDMChannelAsync();
            var e = _lib.createEmbedWithText("EdgyBot", "We hope you enjoyed your stay, " + user.Username + "!");
            await dm.SendMessageAsync("", embed: e);
        }
    }
}
