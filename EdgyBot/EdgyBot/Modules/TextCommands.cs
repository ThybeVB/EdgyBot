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
        [Command("sha512")][Name("sha512")][Summary("Encrypts a message to SHA512")]
        public async Task HashSHA512Cmd([Remainder]string input)
        {
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encrypted SHA512 String", GetHashString(input));
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64e")][Name("b64e")][Alias("b64encode")][Summary("Encodes a message to Base 64.")]
        public async Task B64EncryptCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64d")][Name("b64d")][Alias("b64decode")][Summary("Decrypts a Base 64 Encoded message.")]
        public async Task B64DecodeCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Convert.FromBase64String(input);
            string result = System.Text.Encoding.UTF8.GetString(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Decoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("say")][Name("say")][Summary("Have EdgyBot send your message.")]
        public async Task SayCmd ([Remainder]string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message.\nDon't forget to use **Quotation Marks**!");
                return;
            }
            await ReplyAsync(input);
        }
        [Command("sayd")][Name("sayd")][Alias("saydelete")][Summary("This does the same thing as 'sayd', but deletes the original message.")]
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
        [Command("channelinfo")][Name("channelinfo")][Summary("Gives you info about the channel you are in.")]
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

        [Command("suggest")][Alias("sg")][Name("suggest")][Summary("Sends your suggestion to the owner of the bot.")]
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
        [Command("userinfo")][Name("userinfo")][Summary("Mention an user or leave it empty for a description on the user.")]
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
        [Command("serverinfo")][Name("serverinfo")][Summary("Gives you info about the server you are in.")]
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