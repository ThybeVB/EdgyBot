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
    }
}
