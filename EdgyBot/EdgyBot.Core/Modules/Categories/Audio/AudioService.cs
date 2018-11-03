using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Objects;
using Victoria.Objects.Enums;

namespace EdgyBot.Services
{
      public sealed class AudioService : BaseService
      {
          private LavaNode _lavaNode;
          private readonly ConcurrentDictionary<ulong, (LavaTrack Track, List<ulong> Votes)> _voteSkip;

          public AudioService()
          {
              _voteSkip = new ConcurrentDictionary<ulong, (LavaTrack Track, List<ulong> Votes)>();
          }

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
             }

             player.Play(track);
             return track;
         }

        public async Task<LavaTrack> PlayAsync(ulong guildId, string query)
          {
              var search = Uri.IsWellFormedUriString(query, UriKind.RelativeOrAbsolute)
                  ? await _lavaNode.GetTracksAsync(new Uri(query))
                  : await _lavaNode.SearchYouTubeAsync(query);

              var track = search.Tracks.FirstOrDefault();
              var player = _lavaNode.GetPlayer(guildId);
              if (player.CurrentTrack != null)
              {
                  player.Enqueue(track);
              }

              player.Play(track);
              return track;
          }

          public async Task<string> StopAsync(ulong guildId)
          {
              var leave = await _lavaNode.LeaveAsync(guildId);
              return leave ? "Disconnected!" : "Can't leave when I'm not connected??";
          }

          public string Pause(ulong guildId)
          {
              var player = _lavaNode.GetPlayer(guildId);
              try
              {
                  player.Pause();
                  return $"**Paused:** {player.CurrentTrack.Title}";
              }
              catch
              {
                  return "Not playing anything currently.";
              }
          }

          public string Resume(ulong guildId)
          {
              var player = _lavaNode.GetPlayer(guildId);
              try
              {
                  player.Resume();
                  return $"**Resumed:** {player.CurrentTrack.Title}";
              }
              catch
              {
                  return "Not playing anything currently.";
              }
          }

          public string DisplayQueue(ulong guildId)
          {
              var player = _lavaNode.GetPlayer(guildId);
              try
              {
                  return string.Join("\n", player.Queue[guildId].Select(x => $"=> {x.Title}")) ?? "Your queue is empty.";
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
              catch (ArgumentException arg)
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

          public async Task<string> SkipAsync(ulong guildId, ulong userId)
          {
              var player = _lavaNode.GetPlayer(guildId);
              try
              {
                  var users = (await player.VoiceChannel.GetUsersAsync().FlattenAsync()).Count(x => !x.IsBot);
                  if (!_voteSkip.ContainsKey(guildId))
                      _voteSkip.TryAdd(guildId, (player.CurrentTrack, new List<ulong>()));
                  _voteSkip.TryGetValue(guildId, out var skipInfo);

                  if (!skipInfo.Votes.Contains(userId)) skipInfo.Votes.Add(userId);
                  var perc = (int)Math.Round((double)(100 * skipInfo.Votes.Count) / users);
                  if (perc <= 50) return "More votes needed.";
                  _voteSkip.TryUpdate(guildId, skipInfo, skipInfo);
                  player.Stop();
                  return $"**Skipped:** {player.CurrentTrack.Title}";
              }
              catch
              {
                  return "Not playing anything currently.";
              }
          }

          public async Task ConnectAsync(ulong guildId, IVoiceState state, IMessageChannel channel)
          {
              if (state.VoiceChannel == null)
              {
                  await channel.SendMessageAsync("You aren't connected to any voice channels.");
                  return;
              }

              var player = await _lavaNode.JoinAsync(state.VoiceChannel, channel);
              player.Queue.TryAdd(guildId, new LinkedList<LavaTrack>());
              await channel.SendMessageAsync($"Connected to {state.VoiceChannel}.");
          }

          public async Task<string> DisconnectAsync(ulong guildId)
              => await _lavaNode.LeaveAsync(guildId) ? "Disconnected." : "Not connected to any voice channels.";

          private async Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
          {
              if (player == null)
                  return;
              player.Dequeue(track);
              player.Queue.TryGetValue(player.Guild.Id, out var queue);
              var nextTrack = queue.Count == 0 ? null : queue.First?.Value ?? queue.First?.Next?.Value;
              if (nextTrack == null)
              {
                  await _lavaNode.LeaveAsync(player.Guild.Id);
                  await player.TextChannel.SendMessageAsync("Queue Completed!");
                  return;
              }

              player.Play(nextTrack);
              await player.TextChannel.SendMessageAsync($"**Now Playing:** {track.Title}");
          }

          private async Task OnStuck(LavaPlayer player, LavaTrack track, long arg3)
          {
              player.Dequeue(track);
              player.Enqueue(track);
              await player.TextChannel.SendMessageAsync(
                  $"Track {track.Title} got stuck: {arg3}. Track has been requeued.");
          }

          private async Task OnException(LavaPlayer player, LavaTrack track, string arg3)
          {
              player.Dequeue(track);
              player.Enqueue(track);
              await player.TextChannel.SendMessageAsync(
                  $"Track {track.Title} threw an exception: {arg3}. Track has been requeued.");
          }
      }
  }