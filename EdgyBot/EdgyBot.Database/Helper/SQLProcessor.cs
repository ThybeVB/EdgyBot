using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;

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
                throw new InvalidOperationException("_connection == null");

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

        public string ReadAsync (string selectQuery)
        {
            var connection = DatabaseConnection.connection;

            if (connection.connectionObject.State == ConnectionState.Closed)
                connection.connectionObject.Open();

            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                var selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = selectQuery;
                var reader = selectCommand.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                return result;
            }
        }
    }
}
