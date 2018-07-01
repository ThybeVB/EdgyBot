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

            SqliteTransaction transaction = _connection.connectionObject.BeginTransaction();
            SqliteCommand command = _connection.connectionObject.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = query;
            /*
            insertCommand.CommandText = "INSERT INTO message ( text ) VALUES ( $text )";
            insertCommand.Parameters.AddWithValue("$text", "Hello, World!");

            
              var selectCommand = connection.CreateCommand();
        selectCommand.Transaction = transaction;
        selectCommand.CommandText = "SELECT text FROM message";
        using (var reader = selectCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                var message = reader.GetString(0);
                Console.WriteLine(message);
            }
        }

             */
            await command.ExecuteNonQueryAsync();
            transaction.Commit();
            transaction.Dispose();
            _connection.connectionObject.Dispose();
        }
    }
}
