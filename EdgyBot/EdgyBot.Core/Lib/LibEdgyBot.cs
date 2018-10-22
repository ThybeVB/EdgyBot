using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using Discord;
using Discord.Commands;
using EdgyBot.Core.Handler.API;
using Discord.WebSocket;

namespace EdgyBot.Core.Lib
{
    /// <summary>
    /// The Main Library for used for EdgyBot
    /// </summary>
    public class LibEdgyBot : ModuleBase<ShardedCommandContext>
    {
        public readonly Color EdgyColor = new Color(0xca7f0d);
        private readonly Color moneyGreen = new Color(0x85bb65);

        private WebClient client = new WebClient();
        private Random random = new Random();

        public string GetListToken(Botlist botlist)
        {
            switch (botlist)
            {
                case Botlist.BFD:
                    return Bot.Credentials.bfdToken;
                case Botlist.DISCORDBOTS:
                    return Bot.Credentials.dbToken;
                case Botlist.LISTSPACE:
                    return Bot.Credentials.blsToken;
                case Botlist.DBL:
                    return Bot.Credentials.dblToken;
                case Botlist.DBLCOM:
                    return "Bot " + Bot.Credentials.dblComToken;
            }
            return null;
        }

        public int GetMemberCount()
        {
            int users = 0;
            foreach (SocketGuild guild in Context.Client.Guilds)
            {
                if (guild == null)
                    continue;

                users = users + guild.MemberCount;
            }
            return users;
        }

        /// <summary>
        /// Creates an Embed with defaults and a field.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithText(string title, string text, EmbedFooterBuilder footer = null)
        {
            EmbedBuilder eb = new EmbedBuilder();

            if (footer != null) 
                eb.Footer = footer;
            
            eb.Color = EdgyColor;
            eb.AddField(title, text);
            return eb.Build();
        }

        /// <summary>
        /// Creates an Embed with Text and an image.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithImage(string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = EdgyColor,
                ImageUrl = imgUrl,
            };
            eb.AddField("EdgyBot", text);
            return eb.Build();
        }

        /// <summary>
        /// Creates an embed with a field and an image.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithImage(string title, string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder
            {
                Color = EdgyColor,
                ImageUrl = imgUrl
            };
            eb.AddField(title, text);
            return eb.Build();
        }

        /// <summary>
        /// Creates an Error Embed.
        /// </summary>
        /// <param name="errTitle"></param>
        /// <param name="errText"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithError(string errTitle, string errText)
        {
            EmbedBuilder eb = new EmbedBuilder
            {
                Color = Color.DarkRed
            };
            eb.AddField(errTitle, errText);
            return eb.Build();
        }

        /// <summary>
        /// Creates an Embed with defaults.
        /// </summary>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public EmbedBuilder SetupEmbedWithDefaults(bool footerEnabled = false, string userReq = "")
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder
                {
                    Text = DateTime.UtcNow.ToShortTimeString() + " | " + $"Requested by {userReq}"
                };
                eb.Footer = footer;
            }
            eb.Color = EdgyColor;
            return eb;
        }

        public string GetPermissionsString (GuildPermissions perms) 
        {
            StringBuilder sb = new StringBuilder();
            foreach (GuildPermission permission in perms.ToList()) {
                sb.Append(" ``" + permission.ToString() + "`` ");
            }
            return sb.ToString();
        }

        public string GetRandomMemeData(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken memeArray = jObject["data"]["memes"];
            string randomStr = random.Next(0, memeArray.Count()).ToString();

            string memeArrayUrl = (string)memeArray.SelectToken("[" + randomStr + "].url");
            string name = (string)memeArray.SelectToken("[" + randomStr + "].name");

            return memeArrayUrl + "," + name;
        }

        public string GetRandomDogData(string json)
        {
            JObject jObject = JObject.Parse(json);
            string imgUrl = (string)jObject.SelectToken("message");
            return imgUrl;
        }

        /// <summary>
        /// Encodes a string to Base 64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string EncodeB64(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            string result = Convert.ToBase64String(inputBytes);
            return result;
        }

        /// <summary>
        /// Decodes a string from Base 64
        /// </summary>
        /// <param name="encodedMessage"></param>
        /// <returns></returns>
        public string DecodeB64(string encodedMessage)
        {
            byte[] inputBytes = Convert.FromBase64String(encodedMessage);
            string decodedMessage = Encoding.UTF8.GetString(inputBytes);
            return decodedMessage;
        }

        

        public string GetSHA512String(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetSHA512Hash(inputString))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public string GetSHA256String (string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetSHA256Hash(inputString))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        private byte[] GetSHA512Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private byte[] GetSHA256Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetRandomLetters(int size)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool IsEnglishLetter (char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        public string GetUptime () 
        {
            TimeSpan startTime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            string formatted = startTime.ToString(@"dd\dhh\hmm\mss") + "s";
            return formatted;
        }

        /// <summary>
        /// Logs a LogMessage to the Console.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task Log(LogMessage message)
        {
            LogSeverity sev = message.Severity;
            switch (sev)
            {
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.Write($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] [{sev,-8}]");
            Console.ResetColor();
            Console.WriteLine($" {message.Message}");
            return Task.CompletedTask;
        }

        public Task LavalinkLog(LogMessage message)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] [{"Lava",-8}]");
            Console.ResetColor();
            Console.WriteLine($" {message.Message}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs a Message under the EdgyBot domain.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task EdgyLog(LogSeverity severity, string msg)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] [{"EdgyBot",-8}]");
            Console.ResetColor();

            Console.WriteLine($" {msg}");
            return Task.CompletedTask;
        }
        
    }
}