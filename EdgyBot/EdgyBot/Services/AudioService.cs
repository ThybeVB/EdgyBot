using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using Discord;
using Discord.Audio;
using Discord.WebSocket;

namespace EdgyCore.Services
{
    public class AudioService
    {
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

        public async Task StopAudio(SocketGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                client.Dispose();
            }
        }

        public async Task LeaveAudio(IGuild guild, int exitCode = 0)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                if (exitCode == 0)
                {
                    await _lib.EdgyLog(LogSeverity.Info, $"Disconnected from voice on {guild.Id}.");
                    client.Dispose();
                } else
                {
                    await _lib.EdgyLog(LogSeverity.Info, $"Disconnected from voice on {guild.Id}, Exit Code: {exitCode}");
                    client.Dispose();
                }
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
                await _lib.EdgyLog(LogSeverity.Verbose, $"Starting playback of {path} in {guild.Name}");
                using (Process ffmpeg = CreateStream(path))
                using (AudioOutStream stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try
                    {
                        await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);
                    } finally
                    {
                        await stream.FlushAsync();
                    }
                }
            }
            await LeaveAudio(guild);
            client.Dispose();
        }

        public async Task SendYTAudioAsync (IGuild guild, IMessageChannel channel, string path)
        {
            IAudioClient client;
            int exitCode = 0;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                string url = GetAudioUrl(path);
                Process ffmpeg = CreateStream(url);
                await _lib.EdgyLog(LogSeverity.Verbose, $"Starting playback of {path} in {guild.Name}");
                var output = ffmpeg.StandardOutput.BaseStream;
                var audioStream = client.CreatePCMStream(AudioApplication.Music);
                await output.CopyToAsync(audioStream);

                await audioStream.FlushAsync();
                ffmpeg.WaitForExit();
                exitCode = ffmpeg.ExitCode;
            }
            await LeaveAudio(guild, exitCode);
            client.Dispose();
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

        public string GetAudioUrl(string url)
        {
            return Process.Start(new ProcessStartInfo()
            {
                FileName = @"youtube-dl.exe",
                Arguments = $" -x -g \"{url}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true
            }).StandardOutput.ReadLine();
        }
    }
}
