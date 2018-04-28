using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using ImageProcessor;

namespace EdgyCore.Modules.Categories
{
    public class ImageCommands : ModuleBase<SocketCommandContext>
    {
        [Command("imgtest")][RequireOwner]
        public async Task ImgTest ()
        {
            Attachment img = Context.Message.Attachments.SingleOrDefault();
            if (img == null)
                await ReplyAsync("Please provide an image.");
        }
    }
}
