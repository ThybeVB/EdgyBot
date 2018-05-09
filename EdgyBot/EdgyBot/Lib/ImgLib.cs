using Discord.Commands;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EdgyCore
{
    public class ImgLib
    {
        private WebClient webClient = new WebClient();
        private readonly string fileLocation = "C:/EdgyBot/DownloadedImages/";

        /// <summary>
        /// Downloads an Image and sends it to the Channel. The Image gets deleted afterwards.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DownloadAndSendAsync (Uri url, string fileName, SocketCommandContext context)
        {
            await webClient.DownloadFileTaskAsync(url, fileLocation + fileName);
            await context.Channel.SendFileAsync(fileLocation + fileName);

            File.Delete(fileLocation + fileName);
        }
    }
}
