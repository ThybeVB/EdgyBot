using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Diagnostics;
using Discord.Audio;
using System.IO;

namespace EdgyBot.Modules
{
    public class VoiceCommands : ModuleBase<SocketCommandContext>
    {
        [Command("teststuff", RunMode = RunMode.Async)]
        public async Task TestStuff(IVoiceChannel channel = null)
        {
            await ReplyAsync("Starting...");
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync("You are not in a voice channel!");
                return;
            }
            var audioClient = await channel.ConnectAsync();
            await SendASync(audioClient, "endStart_02.ogg");
        }
        private Process CreateStream (string path)
        {
            ProcessStartInfo ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardInput = true,
            };
            return Process.Start(ffmpeg);
        }
        private async Task SendASync (IAudioClient client, string path)
        {
            Process ffmpeg = CreateStream(path);
            Stream output = ffmpeg.StandardOutput.BaseStream;
            AudioOutStream discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }
    }
}
