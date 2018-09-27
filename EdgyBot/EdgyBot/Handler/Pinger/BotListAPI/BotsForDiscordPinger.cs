using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdgyCore.Handler.Pinger
{
    public class BotsForDiscordPinger
    {
        JsonHelper helper = new JsonHelper("https://botsfordiscord.com/api/bots/" + EdgyBot.Credentials.clientID);
        LibEdgyBot lib = new LibEdgyBot();

        public async Task PostServerCountAsync (int serverCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("server_count", serverCount);

            try { helper.postBotlist(dict, Botlist.BFD); } catch (Exception e)
            {
                await lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to Bots For Discord:\n" + e.Message);
            }
        }
    }
}
