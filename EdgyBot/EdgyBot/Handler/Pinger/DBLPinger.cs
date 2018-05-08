using Discord;
using System.Threading.Tasks;
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
                return;
            
            dblApi = new AuthDiscordBotListApi(373163613390897163, dblToken);
        }

        public async Task UpdateStats (int serverCount)
        {
            try
            {
                IDblSelfBot self = await dblApi.GetMeAsync();
                await self.UpdateStatsAsync(serverCount);
            } catch
            {
                await _lib.EdgyLog(LogSeverity.Warning, "DBL Token Empty, skipping DBL Refresh");
            }
        }
    }
}
