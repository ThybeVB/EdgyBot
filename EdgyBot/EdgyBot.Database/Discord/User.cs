using System.Threading.Tasks;

namespace EdgyBot.Database.Discord
{
    public class User
    {
        private readonly ulong _guildID;
        private readonly ulong _userID;

        public User (ulong guildID, ulong userID)
        {
            _guildID = guildID;
            _userID = userID;
        }

        public async Task<bool> BlacklistUserCmd (ulong userID)
        {
            if (_userID == 0)
                return false;

            SQLProcessor processor = new SQLProcessor(DatabaseConnection.connection);

            return true;
        }
    }
}
