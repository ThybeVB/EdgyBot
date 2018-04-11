using System;
using System.Threading.Tasks;
using DiscordBotList.Models;
using DiscordBotsList.Api;
using EdgyCore;

namespace EdgyBot.Handler
{
    public class DBLPinger
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private AuthDiscordBotListApi dblApi;

        public DBLPinger ()
        {
            string dblToken = _lib.getDBLToken();
            if (string.IsNullOrEmpty(dblToken))
            {
                _lib.EdgyLog(Discord.LogSeverity.Warning, "DBL Token Empty, skipping DBL Refresh");
                return;
            }
            dblApi = new AuthDiscordBotListApi(373163613390897163, dblToken);
        }
        public async Task UpdateStats (int serverCount)
        {
            IDblSelfBot self = await dblApi.GetMeAsync();
            await self.UpdateStatsAsync(serverCount);
        }
    }
}
