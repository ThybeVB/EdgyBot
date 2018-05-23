using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace EdgyCore.Handler.Pinger
{
    public class BotsForDiscordPinger
    {
        JsonHelper helper = new JsonHelper("https://botsfordiscord.com/api/v1/bots/" + EdgyBot.Credientals.clientID);
        LibEdgyBot lib = new LibEdgyBot();

        public async Task SendServerCountAsync ()
        {
            var dict = new Dictionary<string, object>();
            var re = lib.GetServerCount();
            dict.Add("server_count", re);

            try { helper.postDataBotsForDiscord(dict); } catch (Exception e)
            {
                await lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to Bots For Discord:\n" + e.Message);
            }
        }
    }
}
