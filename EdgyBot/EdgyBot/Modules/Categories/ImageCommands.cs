using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using ImageProcessor;
using System.Net;
using System.IO;

namespace EdgyCore.Modules.Categories
{
    public class ImageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        WebClient client = new WebClient();

        [Command("imagecommands"), Alias("image", "images", "imgcommands"), Name("imagecommands"), Summary("Some info on why there currently can be no Image Commands")]
        public async Task ImageCommandsInfo ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Why no Image Commands?", "When an Image Command is executed, The image first gets downloaded to the Machine. There are some problems with this.");
            eb.AddField("Problems?", "The Hosting Service that EdgyBot uses does not allow downloading external files from code. This is why EdgyBot has no Image Commands");
            eb.AddField("Fix?", "Getting a Virtual Private Server (VPS) will resolve this issue, But until that, I can't provide Images.");
            eb.AddField("Edit", "I finally got myself a VPS, working on some image commands :smile:");

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("imgtest", RunMode = RunMode.Async)][RequireOwner]
        public async Task ImgTestCmd ()
        {
            Attachment img = Context.Message.Attachments.FirstOrDefault();
            if (img == null)
            {
                string avUrl = Context.Message.Author.GetAvatarUrl();
                await client.DownloadFileTaskAsync(new Uri(avUrl), "C:/EdgyBot/DownloadedImages/imgtest.png");
                await Context.Channel.SendFileAsync("C:/EdgyBot/DownloadedImages/imgtest.png");
            } else
            {
                await client.DownloadFileTaskAsync(new Uri(img.Url), "C:/EdgyBot/DownloadedImages/imgtest.png");
                await Context.Channel.SendFileAsync("C:/EdgyBot/DownloadedImages/imgtest.png");
            }
            File.Delete("C:/EdgyBot/DownloadedImages/imgtest.png");
        }
    }
}
