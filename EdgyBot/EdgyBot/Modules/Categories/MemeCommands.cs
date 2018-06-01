using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore;
using EdgyCore.Handler.Pinger;

namespace EdgyBot.Modules.Categories
{
    public class MemeCommands : ModuleBase<SocketCommandContext>
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        [Command("randommeme", RunMode = RunMode.Async), Alias("meme", "randommem", "memes")]
        [Name("randommeme"), Summary("Gets a random meme from Imgflip.")]
        public async Task RandomMemeCmd ()
        {
            JsonHelper jsonHelper = new JsonHelper("https://api.imgflip.com/get_memes");

            string get = jsonHelper.getRandomMemeImgFlip();

            string imglink = (string)_lib.GetRandomMemeData(get);
            string[] imgParams = imglink.Split(',');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.Title = imgParams[1];
            eb.ImageUrl = imgParams[0];
            eb.Footer = new EmbedFooterBuilder
            {
                Text = "Memes may be bad, depends on Imgflip's users"
            };

            await ReplyAsync("", embed: eb.Build());
        }
    }
}
