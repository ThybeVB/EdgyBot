using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class HashingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        #region Hashes
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
        #endregion
        [Command("sha512")]
        [Name("sha512")]
        [Summary("Encrypts a message to SHA512")]
        public async Task HashSHA512Cmd([Remainder]string input)
        {
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encrypted SHA512 String", GetHashString(input));
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64e")]
        [Name("b64e")]
        [Alias("b64encode")]
        [Summary("Encodes a message to Base 64.")]
        public async Task B64EncryptCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Encoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("b64d")]
        [Name("b64d")]
        [Alias("b64decode")]
        [Summary("Decrypts a Base 64 Encoded message.")]
        public async Task B64DecodeCmd([Remainder]string input)
        {
            byte[] inputBytes = System.Convert.FromBase64String(input);
            string result = System.Text.Encoding.UTF8.GetString(inputBytes);

            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            e.AddField("Decoded Base64 String", result);
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
    }
}
