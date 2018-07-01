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

        public async Task<bool> OpenConnection ()
        {
            if (_connObj == null)
                throw new InvalidOperationException("Cannot open connection before connecting to the Database. Please use the ConnectAsync function first.");

            try
            {
                await _connObj.OpenAsync();

                Connection connection = new Connection
                {
                    connectionObject = _connObj,
                    isOpened = true
                };
                return true;

            } catch (Exception exception)
            {
                return false;
                throw exception;
            }
        }

        public Task CloseConnection ()
        {
            if (_connObj == null)
                throw new InvalidOperationException("You are not connected to a database.");

            _connObj.Close();

            return Task.CompletedTask;
        }
    }
}
