using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyCore.Services;

namespace EdgyCore.Modules.Categories
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<SocketCommandContext>
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedSounds/";

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
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd ([Remainder]string songName)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, filePath + songName);
        }
    }
}
