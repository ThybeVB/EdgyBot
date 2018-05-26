using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Handler.Pinger;

namespace EdgyCore.Handler
{
    public class EventHandler
    {
        private readonly DiscordSocketClient _client;

        private LibEdgyBot _lib = new LibEdgyBot();
        private DiscordBotsPinger _dbPinger = new DiscordBotsPinger();
        private BotsForDiscordPinger _bfdPinger = new BotsForDiscordPinger();
        private static DBLPinger dblPinger = new DBLPinger();

        public static int MemberCount;
        public static int ServerCount;
        public static SocketUser OwnerUser;

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;
            InitEvents();
        }

        private void InitEvents()
        {
            _client.Log += _lib.Log;

            _client.Ready += Ready;
            _client.JoinedGuild += Client_JoinedGuild;
            _client.LeftGuild += Client_LeftGuild;
            _client.Disconnected += Client_Disconnected;
            _client.UserJoined += Client_UserUpdated;
            _client.UserLeft += Client_UserUpdated;
        }

        public async Task Ready()
        {
            OwnerUser = _client.GetUser(_lib.GetOwnerID());
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();

            await RefreshBot(true);
        }

        private Task Client_UserUpdated(SocketGuildUser arg)
        {
            MemberCount = CalculateMemberCount();
            return Task.CompletedTask;
        }

        private async Task Client_Disconnected(Exception exception)
           => await _lib.EdgyLog(LogSeverity.Critical, $"EDGYBOT HAS SHUT DOWN WITH AN EXCEPTION, \n{exception.Source}: {exception.Message}\n{exception.StackTrace}");

        private async Task Client_JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync($"AYO Thanks for inviting me! To see my commands, use e!help. Hope you enjoy them!");
            ServerCount = _client.Guilds.Count;
            await RefreshBot();
        }

        private async Task Client_LeftGuild(SocketGuild guild)
        {
            ServerCount = _client.Guilds.Count;
            await RefreshBot();
        }

        private int CalculateMemberCount()
        {
            int result = 0;
            foreach (SocketGuild guild in _client.Guilds)
            {
                if (guild == null)
                    continue;
                result = result + guild.MemberCount;
            }
            return result;
        }

        private async Task RefreshBot(bool startup = false)
        {
            string gameStatus = "e!help | EdgyBot for " + _client.Guilds.Count + " servers!";
            await _client.SetGameAsync(gameStatus);
            if (startup) {
                await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
            }

            await _bfdPinger.PostServerCountAsync();
            await _dbPinger.PostServerCountAsync();
            await dblPinger.UpdateStats(_client.Guilds.Count);
        }
    }
}
