using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading.Tasks;

namespace EdgyBot.Database
{
    public class Guild
    {
        private ulong _guildId;

        public Guild (ulong guildId)
        {
            _guildId = guildId;
        }

        public async Task ChangePrefix (string prefix)
        {
            SQLProcessor processor = new SQLProcessor(DatabaseConnection.connection);

            if (CheckIfRegisteredAsync(_guildId))
            {
                await UpdateGuildPrefix(_guildId, prefix);
            } else
            {
                await InsertGuildPrefix(_guildId, prefix);
            }
        }

        public async Task<string> GetPrefix (ulong guildID)
        {
            var connection = DatabaseConnection.connection;

            if (connection.connectionObject.State == ConnectionState.Closed)
                connection.connectionObject.Open();

            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                var selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = $"SELECT prefix FROM guildprefix WHERE guildID={guildID}";
                var reader = selectCommand.ExecuteReader();
                string prefix = "";
                while (reader.Read())
                {
                    prefix = reader.GetString(0);
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                if (string.IsNullOrEmpty(prefix))
                    return "e!";

                return prefix;
            }
        }

        public Task EnableCommand(string name)
        {
            throw new NotImplementedException();
        }

        public Task DisableCommand(string name)
        {
            throw new NotImplementedException();
        }

        private async Task InsertGuildPrefix(ulong guildId, string newPrefix)
        {
            SQLProcessor sql = new SQLProcessor(DatabaseConnection.connection);
            await sql.ExecuteQueryAsync($"INSERT INTO guildprefix (guildID, prefix) VALUES ({guildId}, '{newPrefix}')");
        }

        private async Task UpdateGuildPrefix(ulong guildId, string newPrefix)
        {
            SQLProcessor sql = new SQLProcessor(DatabaseConnection.connection);
            await sql.ExecuteQueryAsync($"UPDATE guildprefix SET prefix='{newPrefix}' WHERE guildID={guildId}");
        }

        private bool CheckIfRegisteredAsync(ulong guildID)
        {
            var connection = DatabaseConnection.connection;

            SQLProcessor processor = new SQLProcessor(connection);
            connection.connectionObject.Open();
            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                var selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = $"SELECT guildID FROM guildprefix WHERE guildID={guildID}";
                var reader = selectCommand.ExecuteReader();
                string msg = "";
                while (reader.Read())
                {
                    msg = reader.GetString(0);
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                bool x = false;
                try
                {
                    ulong id = ulong.Parse(msg);

                    if (id == guildID)
                        x = true;

                } catch
                {
                    x = false;
                }
                return x;
            }
        }
    }
}
