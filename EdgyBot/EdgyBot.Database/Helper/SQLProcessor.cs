using System;
using System.Data;
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

            if (_connection.connectionObject.State == ConnectionState.Closed)
                _connection.connectionObject.Open();

            using (SqliteTransaction transaction = _connection.connectionObject.BeginTransaction())
            {
                SqliteCommand command = _connection.connectionObject.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = query;

                await command.ExecuteNonQueryAsync();

                transaction.Commit();
                _connection.connectionObject.Close();
                transaction.Dispose();
            }
        }
    }
}
