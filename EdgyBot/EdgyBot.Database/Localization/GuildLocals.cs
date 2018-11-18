namespace EdgyBot.Database
{
    public class GuildLocals
    {
        private GuildLocalsData _data;

        private readonly ulong _guildId;

        public GuildLocals (ulong guildId)
        {
            _guildId = guildId;

            _data = new GuildLocalsData(guildId);
        }

        public string GetLocale()
            => _data.ReadGuildLocale();
    }
}
