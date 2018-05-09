using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EdgyCore
{
    public class ImgLib
    {
        private WebClient webClient = new WebClient();
        private readonly string fileLocation = "C:/EdgyBot/DownloadedImages/";

        public async Task DownloadAndSendAsync (Uri url, string fileName, SocketCommandContext context)
        {
            await webClient.DownloadFileTaskAsync(url, fileLocation + fileName);
            await context.Channel.SendFileAsync(fileLocation + fileName);
        }
    }
}
