using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore.Services;

namespace EdgyCore.Modules.Categories
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _service = new AudioService();

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd ()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd ()
        {
            await _service.LeaveAudio(Context.Guild);
        }
    }
}
