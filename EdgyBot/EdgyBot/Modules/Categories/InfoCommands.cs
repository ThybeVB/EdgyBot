using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class InfoCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("channelinfo")]
        [Name("channelinfo")]
        [Summary("Gives you info about the channel you are in.")]
        public async Task ChannelInfoCmd()
        {
            string channelName = Context.Channel.Name;
            string createdAt = Context.Channel.CreatedAt.ToString();
            string channelID = Context.Channel.Id.ToString();

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();

            e.AddField("Channel Name", channelName);
            e.AddField("Channel ID", channelID);
            e.AddField("Created", createdAt);

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("userinfo")]
        [Name("userinfo")]
        [Summary("Mention an user or leave it empty for a description on the user.")]
        public async Task UserInfoCmd(IGuildUser usr = null)
        {
            #region VarRegistration
            string username = null;
            string nickname = null;
            string discriminator = null;
            string userID = null;
            bool isBot = false;
            string status = null;
            string pfpUrl = null;
            string playing = null;
            string createdOn = null;
            #endregion
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            if (usr == null) usr = (IGuildUser)Context.Message.Author;
            
            username = usr.Username;
            SocketGuildUser guildUserMnt = (SocketGuildUser)usr;
            nickname = guildUserMnt.Nickname;
            discriminator = usr.Discriminator;
            userID = usr.Id.ToString();
            isBot = usr.IsBot;
            status = usr.Status.ToString();
            pfpUrl = usr.GetAvatarUrl();
            playing = usr.Activity?.Name;
            createdOn = usr.CreatedAt.ToUniversalTime().ToString();
            
            eb.ThumbnailUrl = pfpUrl;
            eb.AddField("Username", username);
            if (string.IsNullOrEmpty(nickname))
            {
                eb.AddField("Nickname", "None");
            }
            else
            {
                eb.AddField("Nickname", nickname);
            }
            eb.AddField("Discriminator (tag)", discriminator);
            eb.AddField("Status", status);
            eb.AddField("Created At", createdOn);
            if (string.IsNullOrEmpty(playing))
            {
                eb.AddField("Playing", "None");
            }
            else
            {
                eb.AddField("Playing", playing);
            }
            eb.AddField("User ID", userID);
            eb.AddField("Is Bot", isBot.ToString());

            Embed e = eb.Build();
            await ReplyAsync("", embed: e);

        }
        [Command("serverinfo")]
        [Name("serverinfo")]
        [Summary("Gives you info about the server you are in.")]
        public async Task ServerInfoCmd()
        {
            var eb = _lib.SetupEmbedWithDefaults(true);

            #region ServerInfo
            string name = Context.Guild.Name;
            string createdAt = Context.Guild.CreatedAt.ToString();
            string serverGuildIconUrl = Context.Guild.IconUrl;
            string serverId = Context.Guild.Id.ToString();
            string memberCount = Context.Guild.MemberCount.ToString();
            string emoteCount = Context.Guild.Emotes.Count.ToString();
            string roleCount = Context.Guild.Roles.Count.ToString();
            string channelCount = Context.Guild.Channels.Count.ToString();
            #endregion

            eb.ThumbnailUrl = serverGuildIconUrl;
            eb.AddField("Server Name", name);
            eb.AddField("Server ID", serverId);
            eb.AddField("Member Count", memberCount);
            eb.AddField("Emote Count", emoteCount);
            eb.AddField("Role Count", roleCount);
            eb.AddField("Channel Count", channelCount);
            eb.AddField("Created At", createdAt);

            await ReplyAsync("", embed: eb.Build());
        }
        [Command("ping")]
        [Name("ping")]
        [Summary("Checks EdgyBot's speed to the server.")]
        public async Task PingCmd()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            eb.AddField("Response Time", $"{Context.Client.Latency.ToString()} Miliseconds");
            await ReplyAsync("", embed: eb.Build());
        }
        [Command("botinfo")][Name("botinfo")][Summary("Gives you info about the Bot")]
        public async Task BotInfoCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            eb.WithThumbnailUrl("http://i0.kym-cdn.com/photos/images/original/001/256/183/9d5.png");
            eb.AddField("Bot Name", "EdgyBot");
            eb.AddField("Server Count", Context.Client.Guilds.Count);
            eb.AddField("Developer", _lib.getOwnerDiscordName());
            await ReplyAsync("", embed: eb.Build());
        }
    }
}