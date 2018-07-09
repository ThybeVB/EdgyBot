using System;
using System.Threading.Tasks;
using System.Net;
using Discord;
using Discord.Commands;
using ImageSharp;

namespace EdgyCore
{
    public class ImgLib
    {
        private WebClient webClient = new WebClient();
        private readonly string fileLocation = "C:/EdgyBot/DownloadedImages/";

        public async Task DownloadAsync(Uri url, string fileName)
            => await webClient.DownloadFileTaskAsync(url, fileLocation + fileName);

        public Image<Rgba32> OpenImage(string fileName)
        {
            return ImageSharp.Image.Load(fileLocation + fileName);
        }

        public async Task DetermineAttachmentAndDownload(Attachment img, SocketCommandContext Context, string fileName, IUser usr = null)
        {
            if (usr != null)
            {
                await DownloadAsync(new Uri(usr.GetAvatarUrl()), fileName);
                return;
            }

            if (img == null)
            {
                string avUrl = Context.Message.Author.GetAvatarUrl();
                await DownloadAsync(new Uri(avUrl), fileName);
            }
            else
            {
                await DownloadAsync(new Uri(img.Url), fileName);
            }
        }
    }
}
