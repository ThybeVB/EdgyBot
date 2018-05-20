using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using System;
using EdgyCore.Handler.Pinger;

namespace EdgyCore.Handler
{
    public class EventHandler
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private readonly DiscordSocketClient _client;

        private static DBLPinger dblPinger;
        public static SocketUser OwnerUser;

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;

            InitEvents();

            dblPinger = new DBLPinger();
        }

        private void InitEvents()
        {
            _client.Log += _lib.Log;

            _client.Ready += Ready;
            _client.JoinedGuild += Client_JoinedGuild;
            _client.LeftGuild += Client_LeftGuild;
            _client.Disconnected += Client_Disconnected;
        }

        public async Task Ready()
        {
            OwnerUser = _client.GetUser(_lib.GetOwnerID());

            await RefreshBot(true);
        }

        private async Task RefreshBot(bool startup = false)
        {
            string gameStatus = "e!help | EdgyBot for " + _client.Guilds.Count + " servers!";
            await _client.SetGameAsync(gameStatus);
            if (startup) {
                await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
            }
            await dblPinger.UpdateStats(_client.Guilds.Count);
        }

        private async Task Client_Disconnected(Exception exception)
            => await _lib.EdgyLog(LogSeverity.Critical, $"EDGYBOT HAS SHUT DOWN WITH AN EXCEPTION, \n{exception.Source}: {exception.Message}\n{exception.StackTrace}");

        private async Task Client_JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync($"AYO Thanks for inviting me! To see my commands, use e!help. Hope you enjoy them!");
            await RefreshBot();
        }

        private async Task Client_LeftGuild(SocketGuild guild)
            => await RefreshBot();
    }
}
