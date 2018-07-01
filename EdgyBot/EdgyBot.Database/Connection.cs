using Microsoft.Data.Sqlite;

namespace EdgyBot.Database
{
    public class Connection
    {
        public SqliteConnection connectionObject { get; set; }
        public bool isOpened { get; set; }
    }
}
