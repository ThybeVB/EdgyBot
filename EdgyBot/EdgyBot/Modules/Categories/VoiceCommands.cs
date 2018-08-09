using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SharpLink;

namespace EdgyBot.Modules.Categories
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LavalinkManager _lavaManager;

        public VoiceCommands (LavalinkManager lavalinkManager)
        {
            _lavaManager = lavalinkManager;
        }

        [Command("youtube")]
        public async Task LavalinkCmd ([Remainder]string query)
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);

            LavalinkTrack track = await _lavaManager.GetTrackAsync($"ytsearch:{query}");
            await player.PlayAsync(track);

            await _lavaManager.LeaveAsync(Context.Guild.Id);
        }

        [Command("pause")]
        public async Task PauseCmd ()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            await player.PauseAsync();
        }

        [Command("resume")]
        public async Task ResumeCmd()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            await player.ResumeAsync();
        }

        [Command("stop")]
        public async Task StopCmd()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            await player.StopAsync();
            await _lavaManager.LeaveAsync(Context.Guild.Id);
        }

        [Command("setvolume")]
        public async Task SetVolumeCmd(uint volume)
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            await player.SetVolumeAsync(volume);
        }
    }
}
