using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using System;
using System.Threading;
using EdgyCore.Handler;

namespace EdgyCore
{
    public class EventHandler : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private LibEdgyBot _lib = new LibEdgyBot();
        public static SocketUser OwnerUser;

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;
            InitEvents();
            new EdgyPinger();
        }

        private void PingClient()
        {
            //This function is being called every 25 minutes to make sure the bot doesn't go idle.
            _lib.EdgyLog(LogSeverity.Info, "Pinged");
        }

        private void InitEvents()
        {
            _client.Log += _lib.Log;
            _client.Ready += Ready;
            _client.JoinedGuild += Client_JoinedGuild;
            _client.Disconnected += Client_Disconnected;
        }

        public async Task Ready()
        {
            OwnerUser = _client.GetUser(_lib.GetOwnerID());

            string gameStatus = "e!help | EdgyBot for " + _client.Guilds.Count + " servers!";
            await _client.SetGameAsync(gameStatus);
            await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
        }

        private async Task Client_Disconnected(Exception exception)
            => await _lib.EdgyLog(LogSeverity.Critical, "EDGYBOT HAS SHUT DOWN WITH AN EXCEPTION, \n" + exception.Message);

        private static async Task Client_JoinedGuild(SocketGuild guild)
            => await guild.DefaultChannel.SendMessageAsync($"SH*T THANKS FOR INVITING ME M8'S, TO SEE ME COMMANDS, USE **{new LibEdgyBot().GetPrefix()}help**.");
        
    }
}
