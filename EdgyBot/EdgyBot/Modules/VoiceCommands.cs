using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Diagnostics;
using Discord.Audio;
using System.IO;
using System.Collections.Concurrent;

namespace EdgyBot.Modules
{
    public class VoiceCommands : ModuleBase<SocketCommandContext>
    {

        [Command("disc", RunMode = RunMode.Async)]
        public async Task DisconnectCmd ()
        {
            IGuild guild = Context.Guild;

            IAudioClient client = guild.AudioClient;
            await client.StopAsync();
        }
        [Command("connect", RunMode = RunMode.Async)]
        public async Task ConnectCmd(string path = null, IVoiceChannel channel = null)
        {
            await ReplyAsync("Starting...");
            if (path == null) return;
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync("You are not in a voice channel!");
                return;
            }
            IAudioClient client = await channel.ConnectAsync();
            await SendASync(client, path);      
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
        private async Task SendASync (IAudioClient client, string path)
        {
            Process ffmpeg = CreateStream(path);
            Stream output = ffmpeg.StandardOutput.BaseStream;
            AudioOutStream stream = client.CreateDirectPCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(stream);
            await stream.FlushAsync();
        }
    }
}
