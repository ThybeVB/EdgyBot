using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core;
using EdgyBot.Core.Lib;
using Victoria;
using EdgyBot.Services;
using Victoria.Entities;

namespace EdgyBot.Modules
{
    [Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<EbShardContext>
    {
        private AudioService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
    
        public VoiceCommands (AudioService service)
        {
            _service = service;
        }

        private Embed GetTrackInfoEmbed (LavaTrack track, IUser requester)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, $"{requester.Username}#{requester.DiscriminatorValue}");

            eb.WithTitle("Added");
            eb.AddField("Song Title", track.Title);
            eb.AddField("Author", track.Author);
            eb.AddField("Length", track.Length);
            eb.AddField("URL", $"[{track.Title}]({track.Uri.ToString()})");

            return eb.Build();
        }

        [Command("youtube", RunMode = RunMode.Async), Alias("play")]
        [Name("youtube"), Summary("Plays a song from YouTube")]
        public async Task YouTubeCmd ([Remainder]string query)
        {
            if (!_service.Audio.IsConnected(Context.Guild.CurrentUser)) {
                await _service.Audio.ConnectAsync(Context.Guild.Id, (Context.User as IVoiceState), Context.Channel);
            }
            
            LavaTrack track = await _service.PlayYouTubeAsync(Context.Guild.Id, query);
            await ReplyAsync("", embed: GetTrackInfoEmbed(track, Context.User));
        }

        [Command("queue"), Alias("q")]
        public async Task QueueCmd ()
        {
            Embed status = _service.Audio.DisplayQueue(Context.Guild.Id);
            await ReplyAsync("", embed: status);
        }
    
        [Command("stop"), Alias("leave")]
        [Name("stop"), Summary("Stops the currently playing Audio")]
        public async Task StopCmd()
            => await ReplyAsync(await _service.Audio.StopAsync(Context.Guild.Id));

        [Command("skip")]
        public async Task SkipCmd ()
            => await _service.Audio.SkipAsync(Context.Guild.Id, Context.User.Id);

        [Command("setvolume"), Alias("volume")]
        [Name("setvolume"), Summary("Sets the volume of the music.")]
        public async Task SetVolumeCmd(int volume)
        {
            AudioService.PlayState state = await _service.Audio.Volume(Context.Guild.Id, volume);
            if (state == AudioService.PlayState.SUCCESS)
            {
                await ReplyAsync("Volume set to " + volume + "%");
            } else
            {
                await ReplyAsync("Could not set the volume to " + volume + "% Is the number too high?");
            }
        }
    }
}
