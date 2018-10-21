using System;
using System.Collections.Generic;
using System.Text;

namespace EdgyBot.Database.Discord
{
    public class GuildEvents
    {
        private readonly ulong _guildId;

        public GuildEvents (ulong guildId)
        {
            _guildId = guildId;
        }

        public bool IsLogGuild ()
        {
            return false;
        }
    }
}
