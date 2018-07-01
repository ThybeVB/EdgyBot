using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace EdgyBot.Database
{
    public class DatabaseConnection
    {
        private SqliteConnection _connObj;

        string _dbFileName = "";
        string _fullDir = "C:/EdgyBot/Database/";

        public DatabaseConnection (string dbFileName, string fullDir = "")
        {
            _dbFileName = dbFileName;

            if (!string.IsNullOrEmpty(fullDir))
                _fullDir = fullDir;
            
        }

        public Task ConnectAsync ()
        {
            SqliteConnection connection = new SqliteConnection("" +
            new SqliteConnectionStringBuilder
            {
                DataSource = _dbFileName
            });

            if (connection != null)
                _connObj = connection;

            return Task.CompletedTask;
        }

        public async Task<Connection> OpenConnection ()
        {
            try
            {
                await _connObj.OpenAsync();

                Connection connection = new Connection
                {
                    connectionObject = _connObj,
                    isOpened = true
                };
                return connection;

            } catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
