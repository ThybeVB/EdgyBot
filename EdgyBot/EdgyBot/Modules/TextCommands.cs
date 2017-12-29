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
        private libEdgyBot lib = new libEdgyBot();

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
        [Command("help")]
        public async Task HelpCMD(string catergory = null)
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();

            if (catergory != null)
            {
                if (catergory == "categories")
                {
                    EmbedBuilder ebCat = new EmbedBuilder();
                    ebCat.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
                    ebCat.Color = new Color(0x0cc6d3);

                    ebCat.AddField("Category", "Description");
                    ebCat.AddField("geometrydash", "Gives commands related to Geometry Dash.");

                    Embed eCat = ebCat.Build();
                    await dm.SendMessageAsync("", embed: eCat);
                    await ReplyAsync("I sent you a message with the available categories, " + Context.User.Mention + "!");
                }
                if (catergory == "geometrydash")
                {
                    EmbedBuilder gdBuilder = new EmbedBuilder();
                    gdBuilder.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
                    gdBuilder.Color = new Color(0x0cc6d3);
                    gdBuilder.AddField("Bot Prefix", "e!");
                    gdBuilder.AddField("profile", "[NAME], shows info about a player.");
                    Embed gdEmbed = gdBuilder.Build();
                    await ReplyAsync("", embed: gdEmbed);
                }

                return;
            }

            EmbedBuilder e = new EmbedBuilder();
            e.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
            e.Color = new Color(0x0cc6d3);

            e.AddField("**WARNING**", "**EDGYBOT IS STILL IN DEVELOPMENT AND HAS MANY BUGS. IF YOU FIND ANY, MONSTAHHH#9629**");
            e.AddField("Bot Prefix", "e!");
            e.AddField("[TEXT]", "A place where you need to place some text.");
            e.AddField("help", "**OPTIONAL [TEXT]** (e!help categories for available categories)");
            e.AddField("invite", "Get a link to invite the bot to other servers.");
            e.AddField("kys", "Tell EdgyBot to go kill himself.");
            e.AddField("jeff", "[MENTION], **JEFF'S SOMEBODY**");
            e.AddField("lol", "[MENTION] **LOL'S SOMEBODY** not what lol means but ok");
            e.AddField("chance", "[TEXT], Calculates your chances.");
            e.AddField("say", "[TEXT], repeats your message.");
            e.AddField("sayd", "[TEXT], repeats your message and deletes it.");
            e.AddField("channelinfo", "Gives info about the channel you are in.");
            e.AddField("userinfo", "[MENTION], Gives info about you or another user.");
            e.AddField("randomnum", "[number] minimum, [number] maximum, [number] your number");
            e.AddField("sha512", "[TEXT], Hashes a string to SHA512");
            e.AddField("b64e", "[TEXT], encrypts a string to Base64");
            e.AddField("b64d", "[TEXT], decrypts a string from Base64");
            e.AddField("ping", "Checks the speed of the bot.");

            Embed a = e.Build();
            await dm.SendMessageAsync("", embed: a);
            await ReplyAsync("I sent you a list of the commands in DM, " + Context.User.Mention);
        }
        [Command("ping")]
        public async Task PingCMD ()
        {
            Embed e = lib.createEmbedWithText("Response Time", $"{Context.Client.Latency.ToString()} Miliseconds", true);
            await ReplyAsync("", embed: e);
        }
        [Command("invite")]
        public async Task InviteCMD ()
        {
            Embed e = lib.createEmbedWithText("Invite Link", lib.getInviteLink() + "\nThanks for inviting EdgyBot, you're awesome :)");
            await ReplyAsync("", embed: e);
        }
        [Command("kys")]
        public async Task KysCMD ()
        {
            await ReplyAsync("NO GO FUCK YOURSELF");
        }
        [Command("jeff")]
        public async Task JeffCMD (IGuildUser user)
        {
            if (user.Id == Context.User.Id)
            {
                await ReplyAsync("You can't jeff yourself :joy:");
                return;
            }
            string imgUrl = "http://sigmastudios.tk/SigmaFiles/jeff.jpg";
            string textStr = user.Mention + ", You just got jeffed by " + Context.User.Mention;
            Embed e = lib.createEmbedWithImage("Jeff", textStr, imgUrl);

            await ReplyAsync("", embed: e);
            
        }
        [Command("setstatus")]
        public async Task SetStatusCMD(string input)
        {
            if (Context.User.Id == 257247527630274561)
            {
                await Context.Client.SetGameAsync(input);
                await ReplyAsync("Changed Status.");
            } else
            {
                await ReplyAsync("No Permissions.");
            }
        }
        [Command("sha512")]
        public async Task HashSHA512CMD(string input)
        {
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Encrypted SHA512 String", GetHashString(input));
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64e")]
        public async Task B64EncryptCMD(string input)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);

            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Encoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64d")]
        public async Task B64DecodeCMD(string input)
        {
            byte[] inputBytes = System.Convert.FromBase64String(input);
            string result = System.Text.Encoding.UTF8.GetString(inputBytes);

            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Decoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("randomnum")]
        public async Task RandomNumCMD(int min, int max)
        {
            if (min >= max)
            {
                Embed err = lib.createEmbedWithText("EdgyBot", "Invalid number input.");
                await ReplyAsync("", embed: err);
                return;
            }
            Random rand = new Random();
            int result = rand.Next(min, max);
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);

            e.AddField("Random Number Generator", "You got " + result.ToString() + "!");

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("flip")]
        public async Task ReverseCMD(string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message!\nDon't Forget to use **Quotation Marks**!");
                return;
            }
            #region Flip
            char[] chararray = input.ToCharArray();
            Array.Reverse(chararray);
            string reverseTxt = "";
            for (int i = 0; i <= chararray.Length - 1; i++)
            {
                reverseTxt += chararray.GetValue(i);
            }
            input = reverseTxt;
            #endregion
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Reversed Text", input);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("flipcoin")]
        public async Task FlipCoinCMD ()
        {
            Random random = new Random();
            EmbedBuilder a = new EmbedBuilder();
            Embed e = a.Build();
            switch (random.Next(1, 3))
            {
                default:
                    await ReplyAsync("Error.");
                    break;
                case 1:
                    e = lib.createEmbedWithImage("Coinflip", "You Got Heads!", "http://sigmastudios.tk/SigmaFiles/heads.png");
                    break;
                case 2:
                    e = lib.createEmbedWithImage("Coinflip", "You Got Tails!", "http://sigmastudios.tk/SigmaFiles/tails.png");
                    break;
            }
            await ReplyAsync("", embed: e);
        }
        [Command("say")]
        public async Task SayCMD (string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message.\nDon't forget to use **Quotation Marks**!");
                return;
            }
            await ReplyAsync(input);
        }
        [Command("sayd")]
        public async Task SaydCMD (string input = null)
        {
            if (input == null)
            {
                Embed e = lib.createEmbedWithText("Sayd", "Please enter a message.\nDon't forget to use **Quotation Marks**!", true);
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync(input);
                await Context.Message.DeleteAsync();
            }         
        }
        [Command("amigay")]
        public async Task AmIGayCMD()
        {
            Random rand = new Random();
            int num = rand.Next(1, 3);
            switch (num)
            {
                case 1:
                    await ReplyAsync("yes boi");
                    break;
                case 2:
                    await ReplyAsync("no boi");
                    break;
            }
        }
        [Command("e")]
        public async Task Secret01CMD()
        {
            await ReplyAsync("monstah is not gay german");
        }
        [Command("chance")]
        public async Task ChanceCMD(string input)
        {
            Random rand = new Random();
            int num = rand.Next(-1, 100);
            string numStr = num.ToString();

            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Chance", "The chance that " + input + " is " + numStr + "%");
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("lol")]
        public async Task LolCMD(IGuildUser user)
        {
            if (user.Id == Context.Message.Author.Id)
            {
                await ReplyAsync("Nah m8 why would u lol urself");
            } else
            {
                await Context.Message.DeleteAsync();
                await ReplyAsync(user.Mention + " lol");
            }
        }
        [Command("channelinfo")]
        public async Task ChannelInfoCMD()
        {
            string cChannelname = Context.Channel.Name;
            string cCreated = Context.Channel.CreatedAt.ToString();
            string cId = Context.Channel.Id.ToString();

            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);

            e.AddField("Channel Name", cChannelname);
            e.AddField("Channel ID", cId);
            e.AddField("Is NSFW?", Context.Channel.IsNsfw.ToString());
            e.AddField("Created", cCreated);

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("userinfo")]
        public async Task UserInfoCMD (IGuildUser usr = null)
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

            EmbedBuilder eb = new EmbedBuilder();
            EmbedFooterBuilder embedFooterBuilder = new EmbedFooterBuilder();
            eb.Color = new Color(0x0cc6d3);
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
                playing = Context.User.Game.ToString();
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
                playing = usr.Game.ToString();
                createdOn = usr.CreatedAt.ToUniversalTime().ToString();
            }
            eb.ThumbnailUrl = pfpUrl;
            eb.AddInlineField("Username", username);
            if (nickname == null || nickname == "")
            {
                eb.AddInlineField("Nickname", "None");
            } else
            {
                eb.AddInlineField("Nickname", nickname);
            }
            eb.AddInlineField("Discriminator (tag)", discriminator);
            eb.AddInlineField("Status", status);
            if (playing == "" || playing == null)
            {
                eb.AddInlineField("Playing", "None");
            } else
            {
                eb.AddInlineField("Playing", playing);
            }
            eb.AddInlineField("User ID", userID);
            eb.AddInlineField("Is Bot", isBot.ToString());                       
            embedFooterBuilder.Text = "Created on " + createdOn;
            eb.Footer = embedFooterBuilder;

            Embed e = eb.Build();
            await ReplyAsync("", embed: e);

        }
        [Command("stab")]
        public async Task StabCMD (SocketUser usr = null)
        {
            if (usr == null)
            {
                await ReplyAsync("You need to mention an user!\nTry **e!stab @User123**.");
                return;
            }
            string title = "Stab";
            string text = usr.Username + ", you just got stabbed by " + Context.User.Username + "!";
            string imgUrl = "https://media.giphy.com/media/xUySTCy0JHxUxw4fao/giphy.gif";
            Embed e = lib.createEmbedWithImage(title, text, imgUrl);
            await ReplyAsync("", embed: e);
        }
        [Command("gay")]
        public async Task GayCmd(IGuildUser usr)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(usr.Mention + ", ur gay :joy: :ok_hand:");
        }
    }
}