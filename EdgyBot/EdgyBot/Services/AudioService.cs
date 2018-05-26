using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Net;
using YoutubeExtractor;
using Discord;
using Discord.Audio;

namespace EdgyCore.Services
{
    public class AudioService
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedSounds/";

        private LibEdgyBot _lib = new LibEdgyBot();

        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                await _lib.EdgyLog(LogSeverity.Info, $"Connected to Voice Channel on Guild {guild.Id}");
            }
        }

        public async Task<string> FetchVideoUrl(string query)
        {
            string ytUrl = "";

            try
            {
                using (WebClient hui = new WebClient())
                    ytUrl = await hui.DownloadStringTaskAsync("https://www.youtube.com/results?search_query=" + query);
            } catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            string removedShitOne = ytUrl.Remove(0, ytUrl.IndexOf("yt-simple-endpoint style-scope ytd-video-renderer"));
            string finalLink = removedShitOne.Substring(removedShitOne.IndexOf("href") + 2, removedShitOne.IndexOf("\" title="));

            return "https://youtube.com" + finalLink;
        }
        
        public async Task DownloadYoutubeVideo(string url)
        {
            var resolver = await Task.Run(() => DownloadUrlResolver.GetDownloadUrls(url));
            foreach (var cyka in resolver)
                DownloadUrlResolver.DecryptDownloadUrl(cyka);

            var pidor = new VideoDownloader(resolver.ElementAt(0), filePath + resolver.First().Title + "." + resolver.First().AudioExtension);
            await Task.Run(() => pidor.Execute());
        }

        public async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                await _lib.EdgyLog(LogSeverity.Info, $"Disconnected from voice on {guild.Id}.");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }

            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
                using (var ffmpeg = CreateStream(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }
            }

            await LeaveAudio(guild);
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
