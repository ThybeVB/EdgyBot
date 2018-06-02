using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Handler.Pinger;

namespace EdgyCore.Handler
{
    public class EventHandler
    {
        private readonly DiscordShardedClient _client;

        private LibEdgyBot _lib = new LibEdgyBot();
        private DiscordBotsPinger _dbPinger = new DiscordBotsPinger();
        private BotsForDiscordPinger _bfdPinger = new BotsForDiscordPinger();
        private static DBLPinger dblPinger = new DBLPinger();

        public static int MemberCount;
        public static int ServerCount;
        public static DateTime StartTime = DateTime.UtcNow;
        public static SocketUser OwnerUser;

        public EventHandler(DiscordShardedClient client)
        {
            _client = client;
            InitEvents();
        }

        private void InitEvents()
        {
            _client.Log += _lib.Log;
            _client.ShardReady += ShardReady;      
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.ShardDisconnected += Client_Disconnected;
            _client.UserJoined += Client_UserUpdated;
            _client.UserLeft += Client_UserUpdated;
        }

        public async Task ShardReady(DiscordSocketClient client)
        {
            OwnerUser = _client.GetUser(_lib.GetOwnerID());
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();

            await RefreshBotAsync(true);
        }

        private Task Client_UserUpdated(SocketGuildUser arg)
        {
            MemberCount = CalculateMemberCount();
            return Task.CompletedTask;
        }

        private async Task Client_Disconnected(Exception exception, DiscordSocketClient client)
           => await _lib.EdgyLog(LogSeverity.Critical, $"EDGYBOT HAS SHUT DOWN WITH AN EXCEPTION, \n{exception.Source}: {exception.Message}\n{exception.StackTrace}");

        private async Task JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync($"AYO Thanks for inviting me! To see my commands, use e!help. Hope you enjoy them!");
            ServerCount = _client.Guilds.Count;

            await RefreshBotAsync();
        }

        private async Task LeftGuild(SocketGuild guild)
        {
            ServerCount = _client.Guilds.Count;
            await RefreshBotAsync();
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

        private async Task RefreshBotAsync(bool startup = false)
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
