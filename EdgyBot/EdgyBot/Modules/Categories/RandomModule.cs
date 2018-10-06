using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore.Lib;

namespace EdgyBot.Modules
{
    [Name("Random Commands"), Summary("Commands that have different output every time.")]
    public class RandomCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("randomnum")][Name("randomnum")][Summary("Gives you a random number between your 2 numbers.")]
        public async Task RandomNumCmd(int min, int max)
        {
            if (min >= max)
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithText("EdgyBot", "Invalid number input."));
                return;
            }
            Random rand = new Random();
            int result = rand.Next(min, max);
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();

            e.AddField("Random Number Generator", "You got " + result.ToString() + "!");
            await ReplyAsync("", embed: e.Build());
        }

        [Command("flip")][Name("flip")][Summary("Flips your message.")]
        public async Task ReverseCmd([Remainder]string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message!");
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

            await ReplyAsync("", embed: _lib.CreateEmbedWithText("Reversed Text", input));
        }

        [Command("flipcoin")][Name("flipcoin")][Summary("Flips a coin.")]
        public async Task FlipCoinCmd()
        {
            Random random = new Random();
            Embed e = null;
            switch (random.Next(1, 3))
            {
                default:
                    await ReplyAsync("Error.");
                    break;
                case 1:
                    e = _lib.CreateEmbedWithImage("Coinflip", "You Got Heads!", "https://i2.wp.com/thisthingcalledcrypto.com/wp-content/uploads/2018/02/Doge-2.png?fit=296%2C300&ssl=1");
                    break;
                case 2:
                    e = _lib.CreateEmbedWithImage("Coinflip", "You Got Tails!", "https://lh3.googleusercontent.com/OTZ_AxUMM3GBsNSSZazKCJJWbTM7wB0oT2NexK2jAm-dlU1vFxk2YFKhZbvsJfJbx3w=w300");
                    break;
            }
            await ReplyAsync("", embed: e);
        }

        [Command("chance")][Name("chance")][Summary("What chance do you have of x?")]
        public async Task ChanceCmd([Remainder]string input)
        {
            Random rand = new Random();
            int num = rand.Next(-1, 101);
            string numStr = num.ToString();

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Chance", $"The chance that {input} is {numStr}%");
            await ReplyAsync("", embed: e.Build());
        }
    }
}
