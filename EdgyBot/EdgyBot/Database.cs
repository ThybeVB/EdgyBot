using System.Data.SQLite;

namespace EdgyBot
{
    public class Database
    {
        private const string _dbname = "database.db";

        public void ExecuteQuery (string query)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);

            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }       
    }
}
