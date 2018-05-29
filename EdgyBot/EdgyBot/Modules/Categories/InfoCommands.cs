using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyCore;

namespace EdgyBot.Modules.Categories
{
    [Name("Info Commands"), Summary("Commands providing information about a certain thingyyyy")]
    public class InfoCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("uptime"), Alias("up")]
        [Name("uptime"), Summary("Gives you the uptime of EdgyBot")]
        public async Task UptimeCmd () 
        {
            TimeSpan delta = _lib.CalculateUptime();
            string time = $"{delta.Days}:{delta.Hours}:{delta.Minutes}:{delta.Seconds}";

            await ReplyAsync("", embed: _lib.CreateEmbedWithText("EdgyBot Uptime", $"{time} (Last Restart)"));
        }

        [Command("channelinfo")]
        [Name("channelinfo")]
        [Summary("Gives you info about the channel you are in.")]
        public async Task ChannelInfoCmd (IChannel channel = null)
        {
            if (channel == null)
                channel = Context.Channel;
       
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();

            e.AddField("Channel Name", channel.Name);
            e.AddField("Channel ID", channel.Id.ToString());
            e.AddField("Created", channel.CreatedAt.ToString());

            await ReplyAsync("", embed: e.Build());
        }

        [Command("userinfo")]
        [Name("userinfo")]
        [Summary("Mention an user or leave it empty for a description on the user.")]
        public async Task UserInfoCmd(IGuildUser usr = null)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            if (usr == null) usr = (IGuildUser)Context.Message.Author;
            SocketGuildUser guildUserMnt = (SocketGuildUser)usr;

            string username = usr.Username;         
            string nickname = guildUserMnt.Nickname;
            string discriminator = usr.Discriminator;
            string userID = usr.Id.ToString();
            bool isBot = usr.IsBot;
            string status = usr.Status.ToString();
            string pfpUrl = usr.GetAvatarUrl();
            string playing = usr.Activity?.Name;
            string createdOn = usr.CreatedAt.ToUniversalTime().ToString();

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
        [Alias("guildinfo")]
        [Summary("Gives you info about the server you are in.")]
        public async Task ServerInfoCmd()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);

            #region ServerInfo
            string name = Context.Guild.Name;
            string createdAt = Context.Guild.CreatedAt.ToString();
            string serverGuildIconUrl = Context.Guild.IconUrl;
            string serverId = Context.Guild.Id.ToString();
            string memberCount = Context.Guild.MemberCount.ToString();
            string emoteCount = Context.Guild.Emotes.Count.ToString();
            string roleCount = Context.Guild.Roles.Count.ToString();
            string categoriesCount = Context.Guild.CategoryChannels.Count.ToString();
            string channelCount = Context.Guild.Channels.Count.ToString();
            #endregion

            eb.ThumbnailUrl = serverGuildIconUrl;
            eb.AddField("Guild Name", name);
            eb.AddField("Server ID", serverId);
            eb.AddField("Member Count", memberCount);
            eb.AddField("Emote Count", emoteCount);
            eb.AddField("Role Count", roleCount);
            eb.AddField("Category Count", categoriesCount);
            eb.AddField("Channel Count", channelCount);
            eb.AddField("Created At", createdAt);

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("ping")]
        [Name("ping")]
        [Summary("Checks EdgyBot's speed to the server.")]
        public async Task PingCmd()
        {
            EmbedFooterBuilder fb = new EmbedFooterBuilder
            {
                Text = $"Response Time: {Context.Client.Latency.ToString()}ms | {DateTime.Now.ToUniversalTime()} GMT"
            };
            Embed e = _lib.CreateEmbedWithText("Ping", "Pong! :ping_pong:", fb);
            await ReplyAsync("", embed: e);

        }

        [Command("info")][Name("info")][Summary("Gives you info about the Bot")]
        public async Task BotInfoCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            eb.WithThumbnailUrl("http://i0.kym-cdn.com/photos/images/original/001/256/183/9d5.png");
            
            eb.AddField("EdgyBot", "A multipurpose bot with a great variety of commands ranging from fun to well.. not so fun");
            eb.AddField("Library", "Discord.Net");
            eb.AddField("Library Version", "2.0.0-beta2-00951 (API v6)");
            eb.AddField("Server Count", Context.Client.Guilds.Count);
            eb.AddField("Total Users", EdgyCore.Handler.EventHandler.MemberCount);
            eb.AddField("Status", Context.Client.Activity.Name);
            eb.AddField("Uptime", _lib.CalculateUptime());
            eb.AddField("Developer", _lib.GetOwnerDiscordName());

            await ReplyAsync("", embed: eb.Build());
        }
    }
}