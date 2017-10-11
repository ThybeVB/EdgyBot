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
            e.AddField("sha512", "[STRING], Hashes a string to SHA512");
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
            //await ReplyAsync("https://discordapp.com/oauth2/authorize?client_id=366543690660970496&scope=bot&permissions=2146958591");
        }
        [Command("kys")]
        public async Task KysCMD ()
        {
            await ReplyAsync("NO GO FUCK YOURSELF");
        }
        [Command("jeff")]
        public async Task JeffCMD (IGuildUser user)
        {
            if (user.Username == Context.User.Username)
            {
                await ReplyAsync("You can't jeff urself :joy:");
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

        
    }
}
