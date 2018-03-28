using Discord;
using Discord.Commands;
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
        public static string EncodeB64 (string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);
            return result;
        }
        public static string DecodeB64(string encodedInput)
        {
            byte[] inputBytes = System.Convert.FromBase64String(encodedInput);
            string result = Encoding.UTF8.GetString(inputBytes);
            return result;
        }
        #endregion
        [Command("sha512")]
        [Name("sha512")]
        [Summary("Encrypts a message to SHA512")]
        public async Task HashSHA512Cmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Encrypted SHA512 String", GetHashString(input));

            await ReplyAsync("", embed: eb.Build());
        }
        [Command("b64e")]
        [Name("b64e")]
        [Summary("Encodes a message to Base 64.")]
        public async Task B64EncryptCmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Encoded Base64 String", EncodeB64(input));

            await ReplyAsync("", embed: eb.Build());
        }
        [Command("b64d")]
        [Name("b64d")]
        [Summary("Decrypts a Base 64 Encoded message.")]
        public async Task B64DecodeCmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Decoded Base64 String", DecodeB64(input));

            await ReplyAsync("", embed: eb.Build());
        }
    }
}
