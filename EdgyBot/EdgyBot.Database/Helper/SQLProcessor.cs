using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace EdgyBot.Database
{
    public class SQLProcessor
    {
        private Connection _connection;

        public SQLProcessor (Connection connection)
        {
            _connection = connection;
        }

        public async Task ExecuteQueryAsync (string query)
        {
            if (_connection == null)
                throw new InvalidOperationException("Connection NULL");

            var clone = _connection;

            using (SqliteTransaction transaction = clone.connectionObject.BeginTransaction())
            {
                SqliteCommand command = clone.connectionObject.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = query;

                await command.ExecuteNonQueryAsync();

                transaction.Commit();
                transaction.Dispose();
            }
        }
    }
}
