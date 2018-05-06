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

        [Command("imagecommands"), Alias("image", "images", "imgcommands"), Name("imagecommands"), Summary("Some info on why there currently can be no Image Commands")]
        public async Task ImageCommandsInfo ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Why no Image Commands?", "When an Image Command is executed, The image first gets downloaded to the Machine. There are some problems with this.");
            eb.AddField("Problems?", "The Hosting Service that EdgyBot uses does not allow downloading external files from code. This is why EdgyBot has no Image Commands");
            eb.AddField("Fix?", "Getting a Virtual Private Server (VPS) will resolve this issue, But until that, I can't provide Images.");

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("imgtest", RunMode = RunMode.Async)][RequireOwner]
        public async Task ImgTestCmd ()
        {
            Attachment img = Context.Message.Attachments.SingleOrDefault();

            if (img == null)
            {
                await ReplyAsync("Please provide an image.");
                return;
            }
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(img.Url), "UserImg/imgTest.png");
            await Context.Channel.SendFileAsync("UserImg/imgTest.png");
        }
    }
}
