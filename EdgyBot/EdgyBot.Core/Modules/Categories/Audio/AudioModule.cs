using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SharpLink;
using EdgyBot.Core.Lib;
using Victoria;
using EdgyBot.Services;
using System.Collections.Generic;
using Victoria.Objects;

namespace EdgyBot.Modules
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly AudioService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
    
        public VoiceCommands (AudioService service)
        {
            _service = service;
        }

        private Embed GetTrackInfoEmbed (LavalinkTrack track, IUser requester)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, $"{requester.Username}#{requester.DiscriminatorValue}");

            eb.WithTitle("Now Playing");
            eb.AddField("Song Title", track.Title);
            eb.AddField("Author", track.Author);
            eb.AddField("Length", track.Length);

            return eb.Build();
        }
    
        [Command("soundcloud", RunMode = RunMode.Async)]
        [Name("soundcloud"), Summary("Plays a song from Soundcloud")]
        public async Task SoundCloudCmd ([Remainder]string query)
        {
            LavaNode node = _service.GetNode();

            var player = await node.JoinAsync((Context.User as IVoiceState).VoiceChannel, Context.Channel);

            var search = await node.GetTracksAsync($"scsearch:{query}");
            var track = search.Tracks.FirstOrDefault();

            player.Play(track);
        }
    
        [Command("youtube", RunMode = RunMode.Async), Alias("play")]
        [Name("youtube"), Summary("Plays a song from YouTube")]
        public async Task YouTubeCmd ([Remainder]string query)
        {
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            //LoadTracksResponse r = await _lavaManager.GetTracksAsync($"ytsearch:{query}");
            //LavalinkTrack tr = r.Tracks.First();
            //
            //await player.PlayAsync(tr);
            //await ReplyAsync("", embed: GetTrackInfoEmbed(tr, Context.User));
        }

        [Command("httpplay")]
        public async Task HttpPlay ([Remainder]string query)
        {
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id) ?? await _lavaManager.JoinAsync((Context.User as IVoiceState).VoiceChannel);
            //LoadTracksResponse r = await _lavaManager.GetTracksAsync($"ytsearch:{query}");
            //LavalinkTrack tr = r.Tracks.First();
            //
            //await player.PlayAsync(tr);
            //await ReplyAsync("", embed: GetTrackInfoEmbed(tr, Context.User));
        }
    
        [Command("pause")]
        [Name("pause"), Summary("Pause the currently playing Audio")]
        public async Task PauseCmd ()
        {
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            //await player.PauseAsync();
        }
    
        [Command("resume")]
        [Name("resume"), Summary("Resume the currently paused Audio")]
        public async Task ResumeCmd()
        {
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            //await player.ResumeAsync();
        }
    
        [Command("stop")]
        [Name("stop"), Summary("Stops the currently playing Audio")]
        public async Task StopCmd()
        {
            LavaNode node = _service.GetNode();
            var player = node.GetPlayer(Context.Guild.Id);
            player.Stop();
            await player.VoiceChannel.DisconnectAsync();
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            //await player.StopAsync();
            //await _lavaManager.LeaveAsync(Context.Guild.Id);
        }

        [Command("setvolume"), Alias("volume")]
        [Name("setvolume"), Summary("Sets the volume of the music.")]
        public async Task SetVolumeCmd(uint volume)
        {
            //LavalinkPlayer player = _lavaManager.GetPlayer(Context.Guild.Id);
            //await player.SetVolumeAsync(volume);
        }
    }
}
