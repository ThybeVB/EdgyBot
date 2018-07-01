using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;

namespace EdgyBot.Database
{
    public class Guild
    {
        private Connection connection;
        private ulong guildId;

        public Guild (ulong guildId, Connection connection)
        {
            this.guildId = guildId;
            this.connection = connection;
        }

        public async Task ChangePrefix (string prefix)
        {
            SQLProcessor processor = new SQLProcessor(connection);

            if (CheckIfRegisteredAsync(guildId))
            {
                await UpdateGuildPrefix(guildId);
            } else
            {
                await InsertGuildPrefix(guildId);
            }
        }

        private async Task InsertGuildPrefix(ulong guildId)
        {
        }

        private async Task UpdateGuildPrefix(ulong guildId)
        {
        }

        private bool CheckIfRegisteredAsync(ulong guildID)
        {
            SQLProcessor processor = new SQLProcessor(connection);

            SqliteTransaction transaction = connection.connectionObject.BeginTransaction();

            var selectCommand = connection.connectionObject.CreateCommand();
            selectCommand.Transaction = transaction;
            selectCommand.CommandText = "SELECT guildID FROM guildprefix";
            var reader = selectCommand.ExecuteReader();
            string msg = "";
            while (reader.Read())
            {
                msg = reader.GetString(0);
            }

            transaction.Commit();
            transaction.Dispose();
            connection.connectionObject.Dispose();

            if (ulong.Parse(msg) == guildId)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
