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

        public async Task<string[]> GetDisabledCommands () 
        {
            int fields = GetDisabledCommandsInt();
            string[] commands = new string[fields];
            for (int fieldLocal = 0; fieldLocal < fields; fields++) 
            {
                commands[fieldLocal] = GetCommandForField(fieldLocal);
                fieldLocal++;
            }
            return commands;
        }

        private int GetDisabledCommandsInt () 
        {
            Connection connection = DatabaseConnection.connection;

            throw new NotImplementedException();
        }

        private string GetCommandForField (int field) 
        {
            Connection connection = DatabaseConnection.connection;

            throw new NotImplementedException();
        }

        public bool CommandDisabled(string rawCommand)
        {
            Connection connection = DatabaseConnection.connection;

            connection.connectionObject.Open();
            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                SqliteCommand selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = $"SELECT command FROM blacklistedcommands WHERE guildID={_guildId}";

                var reader = selectCommand.ExecuteReader();
                int attempt = 0;
                bool exists = false;
                while (reader.Read())
                {
                    string value = reader.GetString(attempt);
                    if (!string.IsNullOrEmpty(value)) {
                        if (value == rawCommand)
                        {
                            exists = true;
                            transaction.Commit();
                            connection.connectionObject.Close();
                            transaction.Dispose();

                            return exists;
                        }
                        exists = false;
                        transaction.Commit();
                        connection.connectionObject.Close();
                        transaction.Dispose();
                        return exists;
                    } else
                    {
                        attempt++;
                    }
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                return exists;
            }
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

        public string GetPrefix ()
        {
            var connection = DatabaseConnection.connection;

            if (connection.connectionObject.State == ConnectionState.Closed)
                connection.connectionObject.Open();

            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                var selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = $"SELECT prefix FROM guildprefix WHERE guildID={_guildId}";
                var reader = selectCommand.ExecuteReader();
                string prefix = "";
                while (reader.Read())
                {
                    prefix = reader.GetString(0);
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                //Get prefix from Credentials (to-do)
                if (string.IsNullOrEmpty(prefix))
                    return Environment.GetEnvironmentVariable("EdgyBot_Prefix", EnvironmentVariableTarget.User);

                return prefix;
            }
        }

        public async Task EnableCommand(ulong guildID, string cmdName)
        {
            SQLProcessor sql = new SQLProcessor(DatabaseConnection.connection);
            await sql.ExecuteQueryAsync($"DELETE FROM blacklistedcommands WHERE guildID={guildID} and command='{cmdName}'");
        }

        public async Task DisableCommand(ulong guildID, string cmdName)
        {
            SQLProcessor sql = new SQLProcessor(DatabaseConnection.connection);
            if (CommandDisabled(cmdName))
                return;
            
            await sql.ExecuteQueryAsync($"INSERT INTO blacklistedcommands (guildID, command) VALUES ({guildID}, '{cmdName}')");
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
