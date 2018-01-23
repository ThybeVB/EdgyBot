using System;
using System.Data.SQLite;

namespace EdgyBot
{
    public class Database
    {
        /// <summary>
        /// File name of the Database.
        /// </summary>
        private readonly string _dbname = "database.db";

        /// <summary>
        /// Executes a SQL Command to the database.
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery (string query)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Excludes a server from recieving announcements etc...
        /// </summary>
        /// <param name="serverID"></param>
        public void BlacklistServer (ulong serverID)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"INSERT INTO blacklistedservers (serverID) VALUES ('{serverID}')";
            cmd.ExecuteNonQuery();
            conn.Clone();
        }
        /// <summary>
        /// Check if a server is blacklisted from recieving announcements.
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public bool IsServerBlacklisted (ulong serverID)
        {
            string isBlackListed = null;
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"SELECT * FROM blacklistedservers WHERE serverID='{serverID}'";
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                isBlackListed = (string)r["serverID"];
            }
            conn.Close();
            if (string.IsNullOrEmpty(isBlackListed))
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
