using System;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;

namespace EdgyCore.Modules.Categories
{
    public class RandomCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("randomnum")][Name("randomnum")][Summary("Gives you a random number between your 2 numbers.")]
        public async Task RandomNumCmd(int min, int max)
        {
            if (min >= max)
            {
                Embed err = _lib.CreateEmbedWithText("EdgyBot", "Invalid number input.");
                await ReplyAsync("", embed: err);
                return;
            }
            Random rand = new Random();
            int result = rand.Next(min, max);
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();

            e.AddField("Random Number Generator", "You got " + result.ToString() + "!");

            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("flip")][Name("flip")][Summary("Flips your message.")]
        public async Task ReverseCmd([Remainder]string input = null)
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
            Embed a = _lib.CreateEmbedWithText("Reversed Text", input);
            await ReplyAsync("", embed: a);
        }
        [Command("flipcoin")][Name("flipcoin")][Summary("Flips a coin.")]
        public async Task FlipCoinCmd()
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
                    e = _lib.CreateEmbedWithImage("Coinflip", "You Got Heads!", "http://sigmastudios.tk/SigmaFiles/heads.png");
                    break;
                case 2:
                    e = _lib.CreateEmbedWithImage("Coinflip", "You Got Tails!", "http://sigmastudios.tk/SigmaFiles/tails.png");
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
            e.AddField("Chance", "The chance that " + input + " is " + numStr + "%");
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
    }
}
