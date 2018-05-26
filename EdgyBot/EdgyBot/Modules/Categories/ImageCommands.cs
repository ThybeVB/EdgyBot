using System.Linq;
using System.IO;
using System.Threading.Tasks;
using ImageSharp;
using Discord;
using Discord.Commands;

namespace EdgyCore.Modules.Categories
{
    public class ImageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedImages/";

        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private ImgLib imgLib = new ImgLib();

        [Name("pixelate"), Summary("Pixelizes an Image")]
        [Command("pixelate", RunMode = RunMode.Async), Alias("pixelize", "pixel")]
        public async Task PixelCmd (IUser usr = null)
        {
            string fileName = "pixelate.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Resize(480, 320);
            img.Pixelate(10);

            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }
    }
}
