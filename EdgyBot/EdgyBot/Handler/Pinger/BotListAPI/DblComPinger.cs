using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdgyCore.Lib;

namespace EdgyCore.Handler.Pinger
{
    public class DblComPinger
    {
        JsonHelper helper = new JsonHelper($"https://discordbotlist.com/api/bots/{EdgyBot.Credentials.clientID}/stats");
        LibEdgyBot lib = new LibEdgyBot();

        public async Task PostServerCountAsync(int serverCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("guilds", serverCount);
            dict.Add("users", EventHandler.MemberCount);

            try { helper.postBotlist(dict, Botlist.DBLCOM); }
            catch (Exception e)
            {
                await lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to DBL.COM:\n" + e.Message);
            }
        }
    }
}
