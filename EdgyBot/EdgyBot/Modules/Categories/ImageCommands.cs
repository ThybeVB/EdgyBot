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
