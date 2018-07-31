using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdgyCore.Handler.Pinger
{
    public class ListcordPinger
    {
        JsonHelper helper = new JsonHelper($"https://listcord.com/api/bot/{EdgyBot.Credentials.clientID}/guilds");
        private LibEdgyBot _lib = new LibEdgyBot();

        public async Task PostServerCountAsync(int serverCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("guilds", serverCount);

            try { helper.postBotlist(dict, Botlist.LISTCORD); }
            catch (Exception e)
            {
                await _lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to Listcord:\n" + e.Message);
            }
        }
    }
}
