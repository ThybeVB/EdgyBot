using System;
using System.Threading.Tasks;
using EdgyBot.Core.Handler.API;
using EdgyBot.Core.Lib;
using Discord;
using Discord.WebSocket;
using SharpLink;
using EdgyBot.Core.Models;

namespace EdgyBot.Core.Handler
{
    public class EventHandler
    {
        private static DiscordShardedClient _client;
        private LavalinkManager _lavaLink;
        private int _shardsConnected = 0;

        private LibEdgyCore _coreLib = new LibEdgyCore();
        private LibEdgyBot _lib = new LibEdgyBot();

        private DiscordBotsPinger _dbPinger = new DiscordBotsPinger();
        private BotsForDiscordPinger _bfdPinger = new BotsForDiscordPinger();
        private static DBLPinger dblPinger = new DBLPinger();
        private DblComPinger dblComPinger = new DblComPinger();
        private BotListSpacePinger blspPinger = new BotListSpacePinger();
        private EdgyAPI edgyApi = new EdgyAPI();

        public static int MemberCount;
        public static int ServerCount;
        public static SocketUser OwnerUser;

        public EventHandler(DiscordShardedClient client, LavalinkManager lavalink)
        {
            _client = client;
            _lavaLink = lavalink;

            _client.Log += _lib.Log;
            _client.ShardReady += ShardReady;
            _client.ShardDisconnected += ShardDisconnected;
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.UserJoined += UserUpdated;
            _client.UserLeft += UserUpdated;
        }

        public LavalinkManager GetManager ()
        {
            return _lavaLink;
        }

        private async Task ShardReady (DiscordSocketClient client)
        {
            _shardsConnected++;

            if (_shardsConnected == _client.Shards.Count)
            {
                await SetupBot();
                await _lavaLink.StartAsync();
                await _lib.EdgyLog(LogSeverity.Info, $"All Shards Connected ({_client.Shards.Count})");
            }  
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


        public async Task SetupBot ()
        {
            if (OwnerUser == null)
                OwnerUser = _client.GetUser(_coreLib.GetOwnerID());
            
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();
            
            await RefreshBotAsync(true);
        }

        private async Task UserUpdated (SocketGuildUser arg)
        {
            MemberCount = CalculateMemberCount();
            await RefreshBotAsync(false, true);
        }

        private async Task ShardDisconnected (Exception exception, DiscordSocketClient client)
           => await _lib.EdgyLog(LogSeverity.Critical, $"EDGYBOT SHARD {client.ShardId} HAS SHUT DOWN WITH AN EXCEPTION, \n{exception.Source}: {exception.Message}\n{exception.StackTrace}");

        private async Task JoinedGuild(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync($"Ayyy Thanks for inviting me! To see my commands, use e!help. Hope you enjoy them!");
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();

            await RefreshBotAsync();
        }

        private async Task LeftGuild (SocketGuild guild)
        {
            ServerCount = _client.Guilds.Count;
            MemberCount = CalculateMemberCount();

            await RefreshBotAsync();
        }

        public static bool StatusIsCustom = false;

        private async Task RefreshBotAsync (bool startup = false, bool inGuild = false)
        {
            string gameStatus = "e!help | EdgyBot for " + ServerCount + " servers!";

            if (!StatusIsCustom)
                await _client.SetGameAsync(gameStatus);

            if (startup) {
                await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
            } else
            {
                StatsModel stats = new StatsModel();
                stats.shards = new Shard[_client.Shards.Count];
                for (int x = 0; x != _client.Shards.Count; x++)
                {
                    int serverCount = _client.GetShard(x).Guilds.Count;
                    int memCount = GetMembersForShard(x);
                    Shard shard = new Shard
                    {
                        name = $"Shard {x}",
                        server_count = serverCount,
                        user_count = memCount
                    };
                    stats.shards[x] = shard;
                }

                /*Temporary! */
                //await edgyApi.PostStatsAsync(stats);
            }

            /* The event happened in the Guild itself, so we do not need to post our server count. */
            if (inGuild)
                return;

            await _bfdPinger.PostServerCountAsync(ServerCount);
            await _dbPinger.PostServerCountAsync(ServerCount);
            await dblPinger.UpdateDBLStatsAsync(ServerCount);
            await dblComPinger.PostServerCountAsync(ServerCount);
            await blspPinger.PostServerCountAsync(ServerCount);
        }

        private int GetMembersForShard(int shardId)
        {
            int memCount = 0;

            var shard = _client.GetShard(shardId);
            foreach (SocketGuild guild in shard.Guilds)
            {
                memCount = memCount + guild.MemberCount;
            }
            return memCount;
        }
    }
}
