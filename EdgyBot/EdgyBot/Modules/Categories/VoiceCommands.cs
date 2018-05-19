using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore.Services;

namespace EdgyCore.Modules.Categories
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _service;

        public VoiceCommands (AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd ()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
            await ReplyAsync($"Joined Voice on {Context.Guild.Id}");
        }
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd ()
        {
            await _service.LeaveAudio(Context.Guild);
            await ReplyAsync($"Left Voice on {Context.Guild.Id}");
        }
    }
}
