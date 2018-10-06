using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdgyBot.Core.Lib;

namespace EdgyBot.Core.Handler.API
{
    public class DblComPinger
    {
        JsonHelper helper = new JsonHelper($"https://discordbotlist.com/api/bots/{Bot.Credentials.clientID}/stats");
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
