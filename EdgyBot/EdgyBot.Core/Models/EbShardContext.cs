using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyBot.Database;
using EdgyBot.Services;
using Newtonsoft.Json.Linq;

namespace EdgyBot.Core
{
    public class EbShardContext : ShardedCommandContext, ICommandContext
    {
        public new DiscordShardedClient Client;
        private readonly SocketUserMessage msg;

        public EbShardContext(DiscordShardedClient client, SocketUserMessage s) 
            : base(client, s)
        {
            Client = client;
            msg = s;
        }

        public JObject Language => GetLanguage();

        private JObject GetLanguage ()
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.ConnectAsync();
            connection.OpenConnection();

            SocketGuild _guild = (msg.Channel as SocketGuildChannel)?.Guild;
            GuildLocals guild = new GuildLocals(_guild.Id);
            string localeStr = guild.GetLocale();

            switch (localeStr)
            {
                default:
                    return LocalizationService.EnglishUS;
                case "nl_BE":
                    return LocalizationService.DutchBE;
            }
        }

        IDiscordClient ICommandContext.Client => Client;
    }
}
