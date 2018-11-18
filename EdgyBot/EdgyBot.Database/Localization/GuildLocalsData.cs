using Microsoft.Data.Sqlite;
using System.Data;

namespace EdgyBot.Database
{
    public class GuildLocalsData
    {
        private readonly ulong _guildId;

        public GuildLocalsData (ulong guildId)
        {
            _guildId = guildId;
        }

        public string ReadGuildLocale()
        {
            var connection = DatabaseConnection.connection;

            if (connection.connectionObject.State == ConnectionState.Closed)
                connection.connectionObject.Open();

            using (SqliteTransaction transaction = connection.connectionObject.BeginTransaction())
            {
                var selectCommand = connection.connectionObject.CreateCommand();
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = $"SELECT locale FROM guildlocals WHERE guildId={_guildId}";
                var reader = selectCommand.ExecuteReader();
                string locale = "";
                while (reader.Read())
                {
                    locale = reader.GetString(0);
                }

                transaction.Commit();
                connection.connectionObject.Close();
                transaction.Dispose();

                if (string.IsNullOrEmpty(locale))
                    return "en_US";

                return locale;
            }
        }
    }
}
