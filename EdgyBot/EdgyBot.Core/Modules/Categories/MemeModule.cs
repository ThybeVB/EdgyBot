using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core;
using EdgyBot.Core.Lib;
using EdgyBot.Core.Handler.API;

namespace EdgyBot.Modules
{
    [Name("Meme Commands"), Summary("Meme'y Commands. Pictures mainly.")]
    public class MemeCommands : ModuleBase<EbShardContext>
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
                Text = (string)Context.Language["meme"]["takenFrom"]
            };
            eb.ImageUrl = imgUrl;

            await ReplyAsync("", embed: eb.Build());
        }
        [Command("cat", RunMode = RunMode.Async), Alias("neko")]
        public async Task CatCmd()
        {
            JsonHelper helper = new JsonHelper("http://shibe.online/api/cats?count=1&urls=true&httpsUrls=false");
            string get = helper.getJSONFromUrl();
            string[] quoteSplit = get.Split('"');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            if (quoteSplit[1] != "")
                eb.WithImageUrl(quoteSplit[1]);

            eb.WithFooter(new EmbedFooterBuilder
            {
                Text = (string)Context.Language["meme"]["catsApi"]
            });

            await ReplyAsync("", embed: eb.Build());
        }


        [Command("shiba", RunMode = RunMode.Async), Alias("shibe", "shib", "randomshiba", "randomshibe")]
        [Name("shibe"), Summary("Gives you a picture of a shiba")]
        public async Task ShibaCmd ()
        {
            JsonHelper helper = new JsonHelper("http://shibe.online/api/shibes?count=1&urls=true&httpsUrls=false");
            string get = helper.getJSONFromUrl();
            string[] quoteSplit = get.Split('"');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            if (quoteSplit[1] != "")
                eb.WithImageUrl(quoteSplit[1]);

            eb.WithFooter(new EmbedFooterBuilder
            {
                Text = (string)Context.Language["meme"]["shibeApi"]
            });

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("birb", RunMode = RunMode.Async), Alias("bird")]
        public async Task BirdCmd ()
        {
            JsonHelper helper = new JsonHelper("http://shibe.online/api/birds?count=1&urls=true&httpsUrls=false");
            string get = helper.getJSONFromUrl();
            string[] quoteSplit = get.Split('"');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            if (quoteSplit[1] != "")
                eb.WithImageUrl(quoteSplit[1]);

            eb.WithFooter(new EmbedFooterBuilder
            {
                Text = (string)Context.Language["meme"]["birdApi"]
            });

            await ReplyAsync("", embed: eb.Build());
        }
    }
}
