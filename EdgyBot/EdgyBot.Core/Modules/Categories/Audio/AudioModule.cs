using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core.Lib;
using Victoria;
using EdgyBot.Services;
using Victoria.Objects;
using System;

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

        [Command("youtube", RunMode = RunMode.Async), Alias("play")]
        [Name("youtube"), Summary("Plays a song from YouTube")]
        public async Task YouTubeCmd ([Remainder]string query)
        {
            await _service.Audio.ConnectAsync(Context.Guild.Id, (Context.User as IVoiceState), Context.Channel);
            LavaTrack track = await _service.PlayYouTubeAsync(Context.Guild.Id, query);
            await ReplyAsync("", embed: GetTrackInfoEmbed(track, Context.User));
        }
    
        [Command("stop"), Alias("leave")]
        [Name("stop"), Summary("Stops the currently playing Audio")]
        public async Task StopCmd()
        {
            await ReplyAsync(await _service.Audio.StopAsync(Context.Guild.Id));
        }

        [Command("setvolume"), Alias("volume")]
        [Name("setvolume"), Summary("Sets the volume of the music.")]
        public async Task SetVolumeCmd(int volume)
        {
            //LavaNode node = await _service.GetNode(Context.Client);
            //var player = node.GetPlayer(Context.Guild.Id);
            //try
            //{
            //    player.Volume(volume);
            //    await ReplyAsync("Set Volume to " + volume + "%");
            //} catch
            //{
            //    await ReplyAsync("Could not set volume to " + volume + "%");
            //}
        }
    }
}
