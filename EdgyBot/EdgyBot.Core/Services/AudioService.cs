using System;
using System.Linq;
using System.Threading.Tasks;
using HyperEx;
using Victoria;
using Discord;
using Discord.WebSocket;
using Victoria.Entities;
using Victoria.Entities.Enums;

namespace EdgyBot.Services
{
    [Inject]
    public sealed class AudioService : BaseService
    {
        private LavaNode _lavaNode;

        public void Initialize(LavaNode node)
        {
            _lavaNode = node;

            node.TrackStuck += OnStuck;
            node.TrackFinished += OnFinished;
            node.TrackException += OnException;
        }

        public async Task<LavaTrack> PlayYouTubeAsync(ulong guildId, string query)
        {
            var search = await _lavaNode.SearchYouTubeAsync(query);

            var track = search.Tracks.FirstOrDefault();
            var player = _lavaNode.GetPlayer(guildId);
            if (player.CurrentTrack != null)
            {
                player.Queue.Enqueue(track);
                return track;
            }

            await player.PlayAsync(track);
            return track;
        }

        public async Task<LavaTrack> PlayAsync(ulong guildId, string query)
        {
            var search = Uri.IsWellFormedUriString(query, UriKind.RelativeOrAbsolute) ?
             await _lavaNode.GetTracksAsync(query) :
             await _lavaNode.SearchYouTubeAsync(query);

            var track = search.Tracks.FirstOrDefault();
            var player = _lavaNode.GetPlayer(guildId);
            if (player.CurrentTrack != null)
            {
                player.Queue.Enqueue(track);
                return track;
            }

            await player.PlayAsync(track);
            return track;
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var leave = await _lavaNode.DisconnectAsync(guildId);
            return leave ? "Disconnected" : "I'm not connected!";
        }

        public Embed DisplayQueue(ulong guildId)
        {
            var player = _lavaNode.GetPlayer(guildId);
            try
            {
                EmbedBuilder eb = new EmbedBuilder
                {
                    Title = "Queue",
                    Color = new Color(0xca7f0d)
                };

                int trackNum = 0;
                foreach (LavaTrack track in player.Queue.Items)
                {
                    if (track == null)
                        continue;
                    trackNum++;
                    eb.AddField($"{trackNum}.", track.Title);
                }
                return eb.Build();
            }
            catch
            {
                return null;
            }
        }

        public enum PlayState
        {
            SUCCESS,
            FAIL,
            NOTINVOICE
        }

        public async Task<PlayState> Volume(ulong guildId, int vol)
        {
            var player = _lavaNode.GetPlayer(guildId);
            try
            {
                await player.SetVolumeAsync(vol);
                return PlayState.SUCCESS;
            }
            catch (ArgumentException)
            {
                return PlayState.FAIL;
            }
            catch
            {
                return PlayState.NOTINVOICE;
            }
        }

        public bool IsConnected (SocketGuildUser user)
        {
            if (user.VoiceChannel != null) {
                return true;
            }

            return false;
        }

        public async Task<string> SkipAsync(ulong guildId, ulong userId)
        {
            LavaPlayer player = _lavaNode.GetPlayer(guildId);
            await player.SkipAsync();
            return $"**Skipped:** {player.CurrentTrack.Title}";
            
        }

        public async Task ConnectAsync(ulong guildId, IVoiceState state, IMessageChannel channel)
        {
            if (state.VoiceChannel == null)
            {
                await channel.SendMessageAsync("You aren't connected to any voice channels.");
                return;
            }
            await _lavaNode.ConnectAsync(state.VoiceChannel, channel);
        }

        public async Task<string> DisconnectAsync(ulong guildId) 
            => await _lavaNode.DisconnectAsync(guildId) ? "Disconnected." : "Not connected to any voice channels.";

        private async Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            if (reason != TrackReason.Finished)
                return;
            LavaTrack nextTrack = null;

            player.Queue.Remove(track);
            nextTrack = player.Queue.Items.First();

            if (nextTrack is null)
            {
                await player.TextChannel.SendMessageAsync("Queue has been completed!");
                await Lavalink.DefaultNode.DisconnectAsync(player.VoiceChannel.GuildId);
                return;
            }

            await player.PlayAsync(nextTrack);
            await player.TextChannel.SendMessageAsync($"**Now Playing:** {nextTrack.Title}");
        }

        private async Task OnStuck(LavaPlayer player, LavaTrack track, long arg3)
        {
            player.Queue.Dequeue();
            player.Queue.Enqueue(track);
            await player.TextChannel.SendMessageAsync($"Track {track.Title} got stuck: {arg3}. Track has been requeued.");
        }

        private async Task OnException(LavaPlayer player, LavaTrack track, string arg3)
        {
            player.Queue.Dequeue();
            player.Queue.Enqueue(track);
            await player.TextChannel.SendMessageAsync($"Track {track.Title} threw an exception: {arg3}. Track has been requeued.");
        }
    }
}