using System.Linq;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using Discord;
using Discord.Commands;
using EdgyBot.Core;
using EdgyBot.Core.Lib;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace EdgyBot.Modules
{
    [Name("Image Commands"), Summary("Commands for manipulating images.")]
    public class ImageCommands : ModuleBase<EbShardContext>
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedImages/";

        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private ImgLib imgLib = new ImgLib();

        [Name("pixelate"), Summary("Pixelizes an image")]
        [Command("pixelate", RunMode = RunMode.Async), Alias("pixelize", "pixel")]
        public async Task PixelCmd (IUser usr = null)
        {
            string fileName = "pixelate.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Mutate(x => x.Resize(480, 320).Pixelate(10));
            
            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }

        [Command("deepfry", RunMode = RunMode.Async), Alias("df")]
        [Name("deepfry"), Summary("Deepfries an image")]
        public async Task DeepfryCmd (IUser usr = null)
        {
            string fileName = "df.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Mutate(x => x.Contrast(99));
            img.Save(filePath + fileName);

            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }

        [Command("grayscale", RunMode = RunMode.Async), Alias("grey", "greyscale")]
        [Name("grayscale"), Summary("Grayscales an image (black and white)")]
        public async Task GreyscaleCmd (IUser usr = null)
        {
            string fileName = "grayscale.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Mutate(x => x.Grayscale());
            img.Save(filePath + fileName);

            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }

        [Command("blur", RunMode = RunMode.Async)]
        [Name("blur"), Summary("Blurs an image.")]
        public async Task BlurCmd (IUser usr = null)
        {
            string fileName = "blur.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Mutate(x => x.GaussianBlur(5.75f));

            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }

        [Command("sharpen", RunMode = RunMode.Async), Alias("sharp", "sp")]
        [Name("sharpen"), Summary("Sharpens an image.")]
        public async Task SharpenCmd (IUser usr = null)
        {
            string fileName = "sharpen.png";

            Attachment image = Context.Message.Attachments.FirstOrDefault();
            await imgLib.DetermineAttachmentAndDownload(image, Context, fileName, usr);

            Image<Rgba32> img = imgLib.OpenImage(fileName);
            img.Mutate(x => x.GaussianSharpen(6));

            img.Save(filePath + fileName);
            await Context.Channel.SendFileAsync(filePath + fileName);

            img.Dispose();
            File.Delete(filePath + fileName);
        }
    }
}
