using System.Collections.Generic;
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

        [Command("meme", RunMode = RunMode.Async), Alias("randommeme", "randommem", "memes")]
        public async Task RandomMemeCmd ()
        {
            JsonHelper jsonHelper = new JsonHelper("https://api.imgflip.com/get_memes");

            string get = jsonHelper.getRandomMemeImgFlip();

            string imglink = (string)_lib.GetRandomMemeData(get);
            string[] imgParams = imglink.Split(',');

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.Title = imgParams[1];
            eb.ImageUrl = imgParams[0];

            await ReplyAsync("", embed: eb.Build());
        }
    }
}
