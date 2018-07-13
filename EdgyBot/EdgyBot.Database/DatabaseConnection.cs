using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace EdgyBot.Database
{
    public class DatabaseConnection
    {
        private SQLProcessor sql;
        private SqliteConnection _connObj;
        public static Connection connection;

        private string _dbFileName = "EdgyBot.db";
        private string _fullDir = "C:/EdgyBot/Database/";

        public DatabaseConnection (string dbFileName  = "", string fullDir = "")
        {
            if (!string.IsNullOrEmpty(dbFileName)) {
                _dbFileName = dbFileName;
            }

            if (!string.IsNullOrEmpty(fullDir))
                _fullDir = fullDir;
            
        }

        public Task ConnectAsync ()
        {
            SqliteConnection connection = new SqliteConnection("" +
            new SqliteConnectionStringBuilder
            {
                DataSource = _fullDir + _dbFileName
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
                Connection connection = new Connection
                {
                    connectionObject = _connObj,
                };
                DatabaseConnection.connection = connection;

                sql = new SQLProcessor(connection);

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

            try { _connObj.Close(); } catch (Exception e)
            {
                throw e;
            }

            return Task.CompletedTask;
        }

        public Connection getConnObj ()
        {
            return connection;
        }
    }
}
