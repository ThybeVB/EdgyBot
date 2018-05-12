﻿using Discord.Commands;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ImageSharp;
using Discord;

namespace EdgyCore
{
    public class ImgLib
    {
        private WebClient webClient = new WebClient();
        private readonly string fileLocation = "C:/EdgyBot/DownloadedImages/";

        /// <summary>
        /// Downloads an Image.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DownloadAsync(Uri url, string fileName, SocketCommandContext context)
        {
            await webClient.DownloadFileTaskAsync(url, fileLocation + fileName);
        }

        /// <summary>
        /// Properly opens a ImageSharp image (RGBA32)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Image<Rgba32> OpenImage(string fileName)
        {
            Image<Rgba32> img = ImageSharp.Image.Load(fileLocation + fileName);
            return img;
        }

        /// <summary>
        /// Determines if the Attachment is a Profile Picture or an Attachment.
        /// Returns the Attachment after the check.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Context"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DetermineAttachmentAndDownload(Attachment img, SocketCommandContext Context, string fileName, IUser usr = null)
        {
            if (usr != null)
            {
                string usrAvUrl = usr.GetAvatarUrl();
                await DownloadAsync(new Uri(usrAvUrl), fileName, Context);
            }

            if (img == null)
            {
                string avUrl = Context.Message.Author.GetAvatarUrl();
                await DownloadAsync(new Uri(avUrl), fileName, Context);
            }
            else
            {
                await DownloadAsync(new Uri(img.Url), fileName, Context);
            }
        }
    }
}