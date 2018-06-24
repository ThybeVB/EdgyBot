using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using EdgyCore.Handler.Pinger;
using EdgyCore.Lib;

namespace EdgyCore.Handler
{
    public class EventHandler
    {
        private DiscordShardedClient _client;

        private LibEdgyCore _coreLib = new LibEdgyCore();
        private LibEdgyBot _lib = new LibEdgyBot();
        public static int MemberCount;
        public static int ServerCount;
        
        public static SocketUser OwnerUser;

        public EventHandler(DiscordShardedClient client)
        {
            _client = client;
            
            _client.Log += _lib.Log;  
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.ShardDisconnected += ShardDisconnected;
            _client.UserJoined += UserUpdated;
            _client.UserLeft += UserUpdated;
        }

        public async Task SetupBot()
        {
            if (OwnerUser == null)
                OwnerUser = _client.GetUser(_coreLib.GetOwnerID());
            
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();
            
            await RefreshBotAsync(true);
        }

        private Task UserUpdated(SocketGuildUser arg)
        {
            MemberCount = CalculateMemberCount();
            return Task.CompletedTask;
        }

        private async Task ShardDisconnected(Exception exception, DiscordSocketClient client)
           => await _lib.EdgyLog(LogSeverity.Critical, $"EDGYBOT SHARD {client.ShardId} HAS SHUT DOWN WITH AN EXCEPTION, \n{exception.Source}: {exception.Message}\n{exception.StackTrace}");

        private async Task JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync($"Ayyy Thanks for inviting me! To see my commands, use e!help. Hope you enjoy them!");
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();

            await RefreshBotAsync();
        }

        private async Task LeftGuild(SocketGuild guild)
        {
            ServerCount = _client.Guilds.Count;
            await RefreshBotAsync();
        }

        private int CalculateMemberCount()
        {
            int users = 0;
            foreach (SocketGuild guild in _client.Guilds)
            {
                if (guild == null)
                    continue;
                    
                users = users + guild.MemberCount;
            }
            return users;
        }

        private async Task RefreshBotAsync(bool startup = false)
        {
            string gameStatus = "e!help | EdgyBot for " + ServerCount + " servers!";
            await _client.SetGameAsync(gameStatus);

            if (startup) {
                await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
            }

            await _coreLib.UpdateBotLists(ServerCount);
        }
    }
}
