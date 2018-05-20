using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using YoutubeExtractor;

namespace EdgyCore.Services
{
    public class AudioService
    {
        private readonly string filePath = "C:/EdgyBot/DownloadedSounds/";

        private LibEdgyBot _lib = LibEdgyBot.Instance;

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

        public async Task<string> FetchVideoUrlFromSearchListWhichIsCalledWithSomeQuery(string queryToSearchOrJustSomeKeywords)
        {
            string theAllShitThatYoutubeGot = "";

            using (WebClient hui = new WebClient())
                theAllShitThatYoutubeGot = await hui.DownloadStringTaskAsync("https://www.youtube.com/results?search_query=" + queryToSearchOrJustSomeKeywords);

            string removedShitOne = theAllShitThatYoutubeGot.Remove(0, theAllShitThatYoutubeGot.IndexOf("yt-simple-endpoint style-scope ytd-video-renderer"));
            string linkThatINeed = removedShitOne.Substring(removedShitOne.IndexOf("href") + 2, removedShitOne.IndexOf("\" title="));

            return "https://youtube.com" + linkThatINeed;
        }
        
        public async Task DownloadYoutubeVideo(string url)
        {
            var cock = await Task.Run(() => DownloadUrlResolver.GetDownloadUrls(url));
            foreach (var cyka in cock)
                DownloadUrlResolver.DecryptDownloadUrl(cyka);

            var pidor = new VideoDownloader(cock.ElementAt(0), filePath + cock.First().Title + "." + cock.First().AudioExtension);
            pidor.DownloadFinished += Pidor_DownloadFinished;
            pidor.Execute();
        }

        private void Pidor_DownloadFinished(object sender, System.EventArgs e)
        {
            
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
