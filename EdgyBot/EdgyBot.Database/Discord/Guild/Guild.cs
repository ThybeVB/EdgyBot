using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;
using EdgyBot.Database.Discord;

namespace EdgyBot.Database
{
    public class Guild
    {
        private ulong _guildId;

        private GuildData guildData = null;

        public Guild (ulong guildId)
        {
            _guildId = guildId;

            guildData = new GuildData(_guildId);
        }

        public bool CommandDisabled(string rawCommand)
        {
            bool disabled = guildData.CommandDisabled(rawCommand);
            return disabled;
        }

        public async Task ChangePrefix(string prefix)
            => await guildData.ChangePrefix(prefix);

        public string GetPrefix ()
            => guildData.GetPrefix();

        public async Task EnableCommand(string cmdName)
            => await guildData.EnableCommand(cmdName);

        public async Task DisableCommand(string cmdName)
            => await guildData.DisableCommand(cmdName);
    }
}
