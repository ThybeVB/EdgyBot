using System;
using System.Data.SQLite;

namespace EdgyBot
{
    public class Database
    {
        private string _dbname = "database.db";

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
            //TODO
        }
    }
}
