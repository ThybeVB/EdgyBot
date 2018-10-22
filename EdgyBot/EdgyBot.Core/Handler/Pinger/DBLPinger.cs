using System;
using System.Threading.Tasks;
using Discord;
using EdgyBot.Core.Lib;
using System.Collections.Generic;

namespace EdgyBot.Core.Handler.API
{
    public class DBLPinger
    {
        private JsonHelper helper = new JsonHelper("https://discordbots.org/api/bots/" + Bot.Credentials.clientID + "/stats");
        private LibEdgyBot _lib = new LibEdgyBot();

        public async Task PostServerCountAsync(int serverCount, int shardCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("server_count", serverCount);
            dict.Add("shards", shardCount);

            try { helper.postBotlist(dict, Botlist.DBL); }
            catch (Exception e)
            {
                await _lib.EdgyLog(LogSeverity.Error, "Failed to post data to DBL:\n" + e.Message);
            }
        }
    }
}
