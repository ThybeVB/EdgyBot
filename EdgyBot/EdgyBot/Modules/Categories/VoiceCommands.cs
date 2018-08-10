using System;
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

        [Command("soundcloud", RunMode = RunMode.Async)]
        [Name("soundcloud"), Summary("Plays a song from Soundcloud")]
        public async Task SoundCloudCmd ([Remainder]string query)
        {
            LavalinkPlayer p = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            LavalinkTrack tr = await _lavaManager.GetTrackAsync($"scsearch:{query}");

            await p.PlayAsync(tr);
        }

        [Command("youtube", RunMode = RunMode.Async), Alias("play")]
        [Name("youtube"), Summary("Plays a song from YouTube")]
        public async Task YouTubeCmd ([Remainder]string query)
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);

            LavalinkTrack track = await _lavaManager.GetTrackAsync($"ytsearch:{query}");
            await player.PlayAsync(track);
        }

        [Command("pause")]
        [Name("pause"), Summary("Pause the currently playing Audio")]
        public async Task PauseCmd ()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            await player.PauseAsync();
        }

        [Command("resume")]
        [Name("resume"), Summary("Resume the currently paused Audio")]
        public async Task ResumeCmd()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            await player.ResumeAsync();
        }

        [Command("stop")]
        [Name("stop"), Summary("Stops the currently playing Audio")]
        public async Task StopCmd()
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            await player.StopAsync();
            await _lavaManager.LeaveAsync(Context.Guild.Id);
        }

        [Command("setvolume"), Alias("volume")]
        [Name("setvolume"), Summary("Sets the volume of the music.")]
        public async Task SetVolumeCmd(uint volume)
        {
            LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            await player.SetVolumeAsync(volume);
        }
    }
}
