using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core.Lib;
using Victoria;
using EdgyBot.Services;
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

        private Embed GetTrackInfoEmbed (LavaTrack track, IUser requester)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, $"{requester.Username}#{requester.DiscriminatorValue}");

            eb.WithTitle("Now Playing");
            eb.AddField("Song Title", track.Title);
            eb.AddField("Author", track.Author);
            eb.AddField("Length", track.Length);
            eb.AddField("URL", $"[{track.Title}]({track.Uri.ToString()})");

            return eb.Build();
        }
    
        [Command("soundcloud", RunMode = RunMode.Async)]
        [Name("soundcloud"), Summary("Plays a song from Soundcloud")]
        public async Task SoundCloudCmd ([Remainder]string query)
        {
            LavaNode node = _service.GetNode();

            var player = await node.JoinAsync((Context.User as IVoiceState).VoiceChannel, Context.Channel);

            var search = await node.SearchSoundCloudAsync(query);
            var track = search.Tracks.FirstOrDefault();

            player.Play(track);
            await ReplyAsync("", embed: GetTrackInfoEmbed(track, Context.User));
        }

        [Command("youtube", RunMode = RunMode.Async), Alias("play")]
        [Name("youtube"), Summary("Plays a song from YouTube")]
        public async Task YouTubeCmd ([Remainder]string query)
        {
            LavaNode node = _service.GetNode();

            var player = await node.JoinAsync((Context.User as IVoiceState).VoiceChannel, Context.Channel);

            var search = await node.SearchYouTubeAsync(query);
            var track = search.Tracks.FirstOrDefault();

            player.Play(track);
            await ReplyAsync("", embed: GetTrackInfoEmbed(track, Context.User));
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
    
        [Command("stop")]
        [Name("stop"), Summary("Stops the currently playing Audio")]
        public async Task StopCmd()
        {
            LavaNode node = _service.GetNode();
            var player = node.GetPlayer(Context.Guild.Id);
            player.Stop();
            await player.VoiceChannel.DisconnectAsync();
        }

        [Command("setvolume"), Alias("volume")]
        [Name("setvolume"), Summary("Sets the volume of the music.")]
        public async Task SetVolumeCmd(int volume)
        {
            LavaNode node = _service.GetNode();
            var player = node.GetPlayer(Context.Guild.Id);
            try
            {
                player.Volume(volume);
                await ReplyAsync("Set Volume to " + volume + "%");
            } catch
            {
                await ReplyAsync("Could not set volume to " + volume + "%");
            }
        }
    }
}
