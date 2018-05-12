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

        [Name("pixelate"), Summary("Pixelizes an Image")]
        [Command("pixelate", RunMode = RunMode.Async), Alias("pixelize", "pixel")]
        public async Task PixelCmd (int pixelInput = 0)
        {
            string fileName = "pixelate.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Resize(300, 300);
            if (pixelInput == 0)
            {
                img.Pixelate(5);
            } else
            {
                img.Pixelate(pixelInput);
            }

            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }
    }
}
