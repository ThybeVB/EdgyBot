using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore;
using EdgyCore.Handler.Pinger;

namespace EdgyBot.Modules.Categories
{
    [Name("Meme Commands"), Alias("Meme'y Commands. Pictures mainly.")]
    public class MemeCommands : ModuleBase<SocketCommandContext>
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        [Command("randomdog", RunMode = RunMode.Async), Alias("dog", "doggo")]
        [Name("randomdog"), Summary("Gets a random dog from Dog CEO")]
        public async Task RandomDogCmd ()
        {
            string get;
            string imgUrl;

            JsonHelper jsonHelper = new JsonHelper("https://dog.ceo/api/breeds/image/random");
            try
            {
                get = jsonHelper.getRandomDogAPI();
                imgUrl = _lib.GetRandomDogData(get);
            } catch (Exception e)
            {
                await ReplyAsync(e.Message);
                return;
            }

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.Footer = new EmbedFooterBuilder
            {
                Text = "All Pictures are taken from https://dog.ceo/dog-api/"
            };
            eb.ImageUrl = imgUrl;

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("randommeme", RunMode = RunMode.Async), Alias("meme", "randommem", "memes")]
        [Name("randommeme"), Summary("Gets a random meme from Imgflip.")]
        public async Task RandomMemeCmd ()
        {
            JsonHelper jsonHelper = new JsonHelper("https://api.imgflip.com/get_memes");
            string get = jsonHelper.getRandomMemeImgFlip();

            string imglink = _lib.GetRandomMemeData(get);
            string[] imgParams = imglink.Split(',');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.Title = imgParams[1];
            eb.ImageUrl = imgParams[0];
            eb.Footer = new EmbedFooterBuilder
            {
                Text = "Memes may be bad, depends on Imgflip's API"
            };

            await ReplyAsync("", embed: eb.Build());
        }
    }
}
