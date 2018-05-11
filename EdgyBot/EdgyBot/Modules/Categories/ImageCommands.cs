using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Net;
using ImageSharp;
using System.IO;

namespace EdgyCore.Modules.Categories
{
    public class ImageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedImages/";

        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private ImgLib imgLib = new ImgLib();
        private WebClient client = new WebClient();



        [Command("imgtest", RunMode = RunMode.Async)][RequireOwner]
        public async Task ImgTestCmd ()
        {
            Attachment img = Context.Message.Attachments.FirstOrDefault();
            img = await imgLib.DetermineAttachment(img, Context, "imgtest.png");
        }

        [Command("hue")]
        public async Task HueCmd ()
        {
            string fileName = "hue.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            image = await imgLib.DetermineAttachment(image, Context, fileName);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Resize(100, 100);
            img.Hue(100);
            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }
    }
}
