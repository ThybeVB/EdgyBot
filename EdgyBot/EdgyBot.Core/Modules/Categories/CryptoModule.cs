using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core;
using EdgyBot.Core.Lib;

namespace EdgyBot.Modules
{
    [Name("Crypto Commands"), Summary("Encryption commands centered for Developers")]
    public class CryptoCommands : ModuleBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("sha512")]
        [Name("sha512"), Summary("Encrypts a message to SHA512")]
        public async Task Shas512Cmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Encrypted SHA512 String", _lib.GetSHA512String(input));

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("sha256")]
        [Name("sha256"), Summary("Encrypts a message to SHA256")]
        public async Task Sha256Cmd ([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Encrypted SHA256 String", _lib.GetSHA256String(input));

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("b64e")]
        [Name("b64e")]
        [Summary("Encodes a message to Base 64.")]
        public async Task B64EncryptCmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Encoded Base64 String", _lib.EncodeB64(input));

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("b64d")]
        [Name("b64d")]
        [Summary("Decodes a Base 64 Encoded message.")]
        public async Task B64DecodeCmd([Remainder]string input)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            try { input = _lib.DecodeB64(input); } catch
            {
                Embed errEmbed = _lib.CreateEmbedWithError("Base 64 Decoder", "The Base64 input you gave was invalid.");
                await ReplyAsync("", embed: errEmbed);
                return;
            }
            eb.AddField("Decoded Base64 String", input);
            await ReplyAsync("", embed: eb.Build());
        }
    }
}
