﻿using System;
using System.Threading.Tasks;
using DiscordBotsList.Api;
using Discord;
using EdgyBot.Core.Lib;

namespace EdgyBot.Core.Handler.API
{
    public class DBLPinger
    {
        private LibEdgyCore _lib = new LibEdgyCore();
        private LibEdgyBot lib = new LibEdgyBot();

        private AuthDiscordBotListApi dblApi;

        public DBLPinger ()
        {
            string dblToken = _lib.GetDBLToken();
            if (string.IsNullOrEmpty(dblToken))
                return;
            
            dblApi = new AuthDiscordBotListApi(_lib.GetBotId(), dblToken);
        }

        public async Task UpdateDBLStatsAsync (int serverCount)
        {
            try
            {
                IDblSelfBot self = await dblApi.GetMeAsync();
                await self.UpdateStatsAsync(serverCount);
            } catch (Exception e)
            {
                await lib.EdgyLog(LogSeverity.Error, e.Message);
            }
        }
    }
}
