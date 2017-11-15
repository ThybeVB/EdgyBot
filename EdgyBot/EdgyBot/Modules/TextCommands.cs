using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EdgyBot.Modules
{
    public class TextCommands : ModuleBase<SocketCommandContext>
    {
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
        public async Task HelpCMD()
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();

            EmbedBuilder e = new EmbedBuilder();
            e.Author = new EmbedAuthorBuilder().WithIconUrl("").WithName("EdgyBot");
            e.Color = new Color(0x0cc6d3);

            e.AddField("**WARNING**", "**EDGYBOT IS STILL IN DEVELOPMENT, AND WILL BE A PRIVATE BOT UNTIL 1.0**");
            e.AddField("Bot Prefix", "$");
            e.AddField("[TEXT]", "A place where you need to place some text.");
            e.AddField("help", "Shows this message!");
            e.AddField("invite", "Get a link to invite the bot to other servers.");
            e.AddField("kys", "Tell EdgyBot to go kill himself.");
            e.AddField("jeff", "[MENTION], **JEFF'S SOMEBODY**");
            e.AddField("lol", "[MENTION] **LOL'S SOMEBODY** not what lol means but ok");
            e.AddField("chance", "[STRING], Calculates your chances.");
            e.AddField("say", "[STRING], repeats your message.");
            e.AddField("sayd", "[STRING], repeats your message and deletes it.");
            e.AddField("channelinfo", "Gives info about the channel you are in.");
            e.AddField("randomnum", "[number] minimum, [number] maximum, [number] your number");
            e.AddField("sha512", "[STRING], Hashes a string to SHA512");
            e.AddField("b64e", "[STRING], encrypts a string to Base64");
            e.AddField("b64d", "[STRING], decrypts a string from Base64");
            e.AddField("ping", "Checks the speed of the bot.");

            Embed a = e.Build();
            await dm.SendMessageAsync("", embed: a);
            await ReplyAsync("I sent you a list of the commands in DM, " + Context.User.Mention);
        }
        [Command("ping")]
        public async Task PingCMD ()
        {
            await ReplyAsync("Speed: " + Context.Client.Latency.ToString() + "ms");
        }
        [Command("invite")]
        public async Task InviteCMD ()
        {
            await ReplyAsync("wait, u wanna invite me? wOOOOOA " + "(currently disabled cuz in dev)");
            //await ReplyAsync("https://discordapp.com/oauth2/authorize?client_id=373163613390897163&scope=bot&permissions=2146958591");
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
                await ReplyAsync("You can't jeff yopurself :joy:");
                return;
            }

            await Context.Channel.SendFileAsync("jeff.jpg");
            await ReplyAsync(user.Mention + ", You just got jeff'd by " + Context.User.Mention);
            
        }
        [Command("setstatus")]
        public async Task SetStatus(string input)
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
        public async Task EncryptSHA512(string input)
        {
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);
            e.AddField("Encrypted SHA512 String", GetHashString(input));
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64e")]
        public async Task B64Encrypt(string input)
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
        public async Task B64Decode(string input)
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
        public async Task RandomNum (int min, int max)
        {
            Random rand = new Random();
            int result = rand.Next(min, max);
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);

            e.AddField("Random Number Generator", "You got " + result.ToString() + "!");

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("flip")]
        public async Task ReverseCMD(string input)
        {
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
        [Command("say")]
        public async Task SayCMD (string input)
        {
            await ReplyAsync(input);
        }
        [Command("sayd")]
        public async Task SaydCMD (string input)
        {
            await ReplyAsync(input);
            await Context.Message.DeleteAsync();
        }
        [Command("amigay")]
        public async Task AmIGayCMD()
        {
            Random rand = new Random();
            int num = rand.Next(0, 3);
            switch (num)
            {
                default:
                    await ReplyAsync("idk");
                    break;

                case 1:
                    await ReplyAsync("yes boi");
                    break;
                case 2:
                    await ReplyAsync("no boi");
                    break;
            }
        }
        [Command("e")]
        public async Task Secret01()
        {
            await ReplyAsync("monstah is not gay german");
        }
        [Command("chance")]
        public async Task ChaceCMD(string input)
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
        public async Task ChannelInfoCmd()
        {
            string cChannelname = Context.Channel.Name;
            string cCreated = Context.Channel.CreatedAt.ToString();
            string cId = Context.Channel.Id.ToString();

            string isNsfw;
            bool x = Context.Channel.IsNsfw;
            if (x)
            {
                isNsfw = "NSFW";
            } else
            {
                isNsfw = "Not NSFW.";
            }
            EmbedBuilder e = new EmbedBuilder();
            e.Color = new Color(0x0cc6d3);

            e.AddField("Channel Name", cChannelname);
            e.AddField("Channel ID", cId);
            e.AddField("Is NSFW?", isNsfw);
            e.AddField("Created", cCreated);

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
    }
}
