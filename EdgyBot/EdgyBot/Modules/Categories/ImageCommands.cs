using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using ImageProcessor;
using System.Net;

namespace EdgyCore.Modules.Categories
{
    public class ImageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("imgtest", RunMode = RunMode.Async)][RequireOwner]
        public async Task ImgTest ()
        {
            Attachment img = Context.Message.Attachments.SingleOrDefault();

            if (img == null)
            {
                await ReplyAsync("Please provide an image.");
                return;
            }
            try { _lib.DownloadImageAsync(img.Url); } 
             catch {
                await ReplyAsync("err");
            }
        }
    }
}
