using System;
using System.Linq;
using System.Threading.Tasks;
using HyperEx;
using Victoria;
using Victoria.Objects;
using Victoria.Objects.Enums;
using Discord;
using Discord.WebSocket;

namespace EdgyBot.Services
{
    [Inject]
    public sealed class AudioService : BaseService
    {
        private LavaNode _lavaNode;

        public AudioService () {}

        public void Initialize(LavaNode node)
        {
            _lavaNode = node;

            node.Stuck += OnStuck;
            node.Finished += OnFinished;
            node.Exception += OnException;
        }

        public async Task<LavaTrack> PlayYouTubeAsync(ulong guildId, string query)
        {
            var search = await _lavaNode.SearchYouTubeAsync(query);

            var track = search.Tracks.FirstOrDefault();
            var player = _lavaNode.GetPlayer(guildId);
            if (player.CurrentTrack != null)
            {
                player.Enqueue(track);
                return track;
            }

            player.Play(track);
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
                player.Enqueue(track);
                return track;
            }

            player.Play(track);
            return track;
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var leave = await _lavaNode.LeaveAsync(guildId);
            return leave ? "Disconnected" : "I'm not connected!";
        }

        public string DisplayQueue(ulong guildId)
        {
            var player = _lavaNode.GetPlayer(guildId);
            try
            {
                return string.Join("\n", player.Queue.Select(x => $"=> {x.Title}")) ?? "Your queue is empty.";
            }
            catch
            {
                return "Your queue is empty.";
            }
        }

        public enum PlayState
        {
            SUCCESS,
            FAIL,
            NOTINVOICE
        }

        public PlayState Volume(ulong guildId, int vol)
        {
            var player = _lavaNode.GetPlayer(guildId);
            try
            {
                player.Volume(vol);
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

        public string Seek(ulong guildId, TimeSpan span)
        {
            var player = _lavaNode.GetPlayer(guildId);
            try
            {
                player.Seek(span);
                return $"**Seeked:** {player.CurrentTrack.Title}";
            }
            catch
            {
                return "Not playing anything currently.";
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
            player.Skip();

            return $"**Skipped:** {player.CurrentTrack.Title}";
            
        }

        public async Task ConnectAsync(ulong guildId, IVoiceState state, IMessageChannel channel)
        {
            if (state.VoiceChannel == null)
            {
                await channel.SendMessageAsync("You aren't connected to any voice channels.");
                return;
            }
            

            var player = await _lavaNode.JoinAsync(state.VoiceChannel, channel);

            //player.Queue.TryAdd(guildId, new LinkedList<LavaTrack>());
        }

        public async Task<string> DisconnectAsync(ulong guildId) => await _lavaNode.LeaveAsync(guildId) ? "Disconnected." : "Not connected to any voice channels.";

        private async Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            if (player == null)
                return;
            player.Dequeue(track);
            var queue = player.Queue;
            var nextTrack = queue.Count == 0 ? null : queue.First?.Value ?? queue.First?.Next?.Value;

            if (nextTrack == null)
            {
                await _lavaNode.LeaveAsync(player.Guild.Id);
                return;
            }

            player.Play(nextTrack);
        }

        private async Task OnStuck(LavaPlayer player, LavaTrack track, long arg3)
        {
            player.Dequeue(track);
            player.Enqueue(track);
            await player.TextChannel.SendMessageAsync($"Track {track.Title} got stuck: {arg3}. Track has been requeued.");
        }

        private async Task OnException(LavaPlayer player, LavaTrack track, string arg3)
        {
            player.Dequeue(track);
            player.Enqueue(track);
            await player.TextChannel.SendMessageAsync($"Track {track.Title} threw an exception: {arg3}. Track has been requeued.");
        }
    }
}