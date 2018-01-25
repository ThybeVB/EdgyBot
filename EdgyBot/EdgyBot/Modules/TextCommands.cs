using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System;
using System.Security.Cryptography;
using System.Text;
using Discord.WebSocket;

namespace EdgyBot.Modules
{
    public class TextCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly Database _database = new Database();
        private DiscordSocketClient _client;

        #region Hashes
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
        #endregion
        [Command("help")][Name("Help")][Summary("Gives EdgyBot's commands and what they do.")]
        public async Task HelpCmd(string category = null)
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();
        
            if (category != null)
            {
                if (category == "categories")
                {
                    EmbedBuilder ebCat = new EmbedBuilder();
                    ebCat.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
                     ebCat.Color = _lib.LightBlue;
        
                    ebCat.AddField("Category", "Description");
                    ebCat.AddField("geometrydash", "Gives commands related to Geometry Dash.");
                    ebCat.AddField("admin", "Gives commands for people with the Administrator permission.");
        
                    Embed eCat = ebCat.Build();
                    await dm.SendMessageAsync("", embed: eCat);
                    await ReplyAsync("I sent you a message with the available categories, " + Context.User.Mention + "!");
                } else if (category == "geometrydash")
                {
                    EmbedBuilder gdBuilder = new EmbedBuilder();
                    gdBuilder.Author = new EmbedAuthorBuilder().WithIconUrl("https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300").WithName("EdgyBot");
                    gdBuilder.Color = _lib.LightBlue;
                    gdBuilder.AddField("Bot Prefix", _lib.GetPrefix());
                    gdBuilder.AddField("profile", "[NAME], shows info about a player.");
                    gdBuilder.AddField("top10players", "Shows the Top 10 leaderboard.");
                    gdBuilder.AddField("topplayers", "[NUMBER], Shows the leaderboard based on your number.");
                    gdBuilder.AddField("topcreators", "[NUMBER] Shows the Creator leaderboard based on your number.");
                    gdBuilder.AddField("top10creators", "Shows the Top 10 creators.");
                    Embed gdEmbed = gdBuilder.Build();
                    await ReplyAsync("", embed: gdEmbed);
                } else if (category == "admin")
                 {
                     EmbedBuilder adminBuilder = new EmbedBuilder();
                     adminBuilder.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
                     adminBuilder.Color = _lib.LightBlue;
                     adminBuilder.AddField("Bot Prefix", _lib.GetPrefix());
                     adminBuilder.AddField("stopannounce", "Stop the server from receiving announcements.");
                     adminBuilder.AddField("blacklisted", "[SERVER ID], Checks if a server is blacklisted.");                    
                     Embed adminEmbed = adminBuilder.Build();
                     await ReplyAsync("", embed: adminEmbed);
                 }
                 return;
            }
        
            EmbedBuilder e = new EmbedBuilder();
            e.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
             e.Color = _lib.LightBlue;
        
            e.AddField("**WARNING**", "**EDGYBOT IS STILL IN DEVELOPMENT AND HAS MANY BUGS. IF YOU FIND ANY, MONSTAHHH#9629**");
            e.AddField("Bot Prefix", _lib.GetPrefix());
            e.AddField("[TEXT]", "A place where you need to place some text.");
            e.AddField("help", "**OPTIONAL [TEXT]** (e!help categories for available categories)");
            e.AddField("invite", "Get a link to invite the bot to other servers.");
            e.AddField("kys", "Tell EdgyBot to go kill himself.");
            e.AddField("jeff", "[MENTION], **JEFF'S SOMEBODY**");
            e.AddField("lol", "[MENTION] **LOL'S SOMEBODY** not what lol means but ok");
            e.AddField("chance", "[TEXT], Calculates your chances.");
            e.AddField("say", "[TEXT], repeats your message.");
            e.AddField("sayd", "[TEXT], repeats your message and deletes it.");
            e.AddField("vertical", "[TEXT], converts a message to vertical text.");
            e.AddField("channelinfo", "Gives info about the channel you are in.");
            e.AddField("userinfo", "[MENTION], Gives info about you or another user.");
            e.AddField("randomnum", "[number] minimum, [number] maximum, [number] your number");
            e.AddField("sha512", "[TEXT], Hashes a string to SHA512");
            e.AddField("b64e", "[TEXT], encrypts a string to Base64");
            e.AddField("b64d", "[TEXT], decrypts a string from Base64");
            e.AddField("ping", "Checks the response time of the bot.");
            e.AddField("stop", "[MENTION], tells somebody to stop.");
        
            Embed a = e.Build();
            await dm.SendMessageAsync("", embed: a);
            await Context.Message.AddReactionAsync(new Emoji("📫"));
        } 
        [Command("ping")][Name("ping")][Summary("Checks EdgyBot's speed to the server.")]
        public async Task PingCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true);
            eb.AddField("Response Time", $"{Context.Client.Latency.ToString()} Miliseconds");

            Embed a = eb.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("invite")][Name("invite")][Summary("Gives the Invite Link for EdgyBot.")]
        public async Task InviteCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Invite Link", _lib.GetInviteLink() + "\nThanks for inviting EdgyBot, you're awesome :)");
            await ReplyAsync("", embed: e);
        }
        [Command("kys")][Name("kys")][Summary("Tells EdgyBot to go kill himself.")]
        public async Task KysCmd ()
        {
            await ReplyAsync("NO GO FUCK YOURSELF");
        }
        //Go from here
        [Command("setstatus")]
        public async Task SetStatusCmd([Remainder]string input)
        {
            if (Context.User.Id == _lib.GetOwnerID())
            {
                await Context.Client.SetGameAsync(input);
                await ReplyAsync("Changed Status.");
            } else
            {
                await ReplyAsync("No Permissions.");
            }
        }
        [Command("sha512")]
        public async Task HashSHA512Cmd([Remainder]string input)
        {
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encrypted SHA512 String", GetHashString(input));
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64e")]
        public async Task B64EncryptCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64d")]
        public async Task B64DecodeCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Convert.FromBase64String(input);
            string result = System.Text.Encoding.UTF8.GetString(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Decoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("say")]
        public async Task SayCmd ([Remainder]string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message.\nDon't forget to use **Quotation Marks**!");
                return;
            }
            await ReplyAsync(input);
        }
        [Command("sayd")]
        public async Task SaydCmd ([Remainder]string input = null)
        {
            if (input == null)
            {
                Embed e = _lib.CreateEmbedWithText("Sayd", "Please enter a message.\nDon't forget to use **Quotation Marks**!", true);
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync(input);
                await Context.Message.DeleteAsync();
            }         
        }    
        [Command("e")]
        public async Task Secret01Cmd()
        {
            await ReplyAsync("monstah is not gay german");
        }
        [Command("channelinfo")]
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

        [Command("suggest")][Alias("sg")]
        public async Task SuggestCmd ([Remainder]string msg = null)
        {
            if (msg != null)
            {
                await EventHandler.OwnerUser.SendMessageAsync(msg);
                await ReplyAsync("Your message has been sent!");
            }
            else
            {
                await ReplyAsync("Please enter a message.");
            }      
        }
        [Command("userinfo")]
        public async Task UserInfoCmd (IGuildUser usr = null)
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
            if (usr == null)
            {
                username = Context.User.Username;
                SocketGuildUser guildUser = (SocketGuildUser)Context.User;
                nickname = guildUser.Nickname;
                discriminator = Context.User.Discriminator;
                userID = Context.User.Id.ToString();
                isBot = Context.User.IsBot;
                status = Context.User.Status.ToString();
                pfpUrl = Context.User.GetAvatarUrl();
                playing = Context.User.Activity.Name;
                createdOn = Context.User.CreatedAt.LocalDateTime.ToLongTimeString();
            } else
            {
                username = usr.Username;
                SocketGuildUser guildUserMnt = (SocketGuildUser)usr;
                nickname = guildUserMnt.Nickname;
                discriminator = usr.Discriminator;
                userID = usr.Id.ToString();
                isBot = usr.IsBot;
                status = usr.Status.ToString();
                pfpUrl = usr.GetAvatarUrl();
                playing = usr.Activity.Name;
                createdOn = usr.CreatedAt.ToUniversalTime().ToString();
            }
            eb.ThumbnailUrl = pfpUrl;
            eb.AddField("Username", username);
            if (string.IsNullOrEmpty(nickname))
            {
                eb.AddField("Nickname", "None");
            } else
            {
                eb.AddField("Nickname", nickname);
            }
            eb.AddField("Discriminator (tag)", discriminator);
            eb.AddField("Status", status);
            eb.AddField("Created At", createdOn);
            if (string.IsNullOrEmpty(playing))
            {
                eb.AddField("Playing", "None");
            } else
            {
                eb.AddField("Playing", playing);
            }
            eb.AddField("User ID", userID);
            eb.AddField("Is Bot", isBot.ToString());                       

            Embed e = eb.Build();
            await ReplyAsync("", embed: e);

        }
        [Command("serverinfo")]
        public async Task ServerInfoCmd()
        {
            var eb = _lib.SetupEmbedWithDefaults(true);         

            string name = Context.Guild.Name;
            string createdAt = Context.Guild.CreatedAt.ToString();
            string serverGuildIconUrl = Context.Guild.IconUrl;
            string serverId = Context.Guild.Id.ToString();
            string memberCount = Context.Guild.MemberCount.ToString();
            string emoteCount = Context.Guild.Emotes.Count.ToString();
            string roleCount = Context.Guild.Roles.Count.ToString();
            string channelCount = Context.Guild.Channels.Count.ToString();
            eb.ThumbnailUrl = serverGuildIconUrl;

            eb.AddField("Server Name", name);
            eb.AddField("Server ID", serverId);
            eb.AddField("Member Count", memberCount);
            eb.AddField("Emote Count", emoteCount);
            eb.AddField("Role Count", roleCount);
            eb.AddField("Channel Count", channelCount);
            eb.AddField("Created At", createdAt);

            Embed a = eb.Build();
            await ReplyAsync("", embed: a);
        }
    }
}