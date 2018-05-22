using Discord;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Discord.WebSocket;
using System.Net;
using EdgyCore.Handler;
using YoutubeExtractor;
using EdgyBot.Handler;

namespace EdgyCore
{
    /// <summary>
    /// The Main Library for used for EdgyBot, Depends on some discord stuff and other stuff.
    /// </summary>
    public class LibEdgyBot : ModuleBase<SocketCommandContext>
    {
        public readonly Color LightBlue = new Color(0x0cc6d3);
        private readonly Color moneyGreen = new Color(0x85bb65);
        private WebClient client = new WebClient();

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
            
            eb.Color = LightBlue;
            eb.AddField(title, text);
            return eb.Build();
        }

        public Credentials GetCredientals ()
        {
            CredientalsManager manager = new CredientalsManager();

            string isSetup = Environment.GetEnvironmentVariable("EdgyBot_IsSetup", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(isSetup) || isSetup == "y")
            {
                return manager.Read(true);
            } else
            {
                return manager.Read(false);
            }
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
                Color = LightBlue,
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
                Color = LightBlue,
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
        public EmbedBuilder SetupEmbedWithDefaults(bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder
                {
                    Text = DateTime.Now.ToUniversalTime().ToString() + "|" + "EdgyBot"
                };
                eb.Footer = footer;
            }
            eb.Color = LightBlue;
            return eb;
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

        private byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetSHA512String(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public ulong GetBotId()
        {
            return EdgyBot.Credientals.clientID;
        }

        public string GetDBLToken()
        {
            string dblToken = EdgyBot.Credientals.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }

        public string GetInviteLink()
        {
            string invLink = EdgyBot.Credientals.invLink;
            if (string.IsNullOrEmpty(invLink)) {
                string clientID = GetClientID().ToString();
                invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=" + clientID; 
            }
            return invLink;
        }

        public string GetToken()
        {
            return EdgyBot.Credientals.token;
        }

        public string GetRandomLetters(int size)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        public string GetPrefix()
        {
            string prefix = EdgyBot.Credientals.prefix;
            if (string.IsNullOrEmpty(prefix)) return "e!";
            return prefix;
        }

        public string GetGJP()
        {
            string gjp = EdgyBot.Credientals.GJP;
            if (string.IsNullOrEmpty(gjp)) return null;
            return gjp;
        }

        public ulong GetClientID ()
        {
            ulong clientID = EdgyBot.Credientals.clientID;
            return clientID;
        }

        public string GetGDAccID()
        {
            string accID = EdgyBot.Credientals.accID;
            if (string.IsNullOrEmpty(accID)) return null;
            return EdgyBot.Credientals.accID;
        }

        public string GetOwnerDiscordName()
        {
            SocketUser ownerUser = Handler.EventHandler.OwnerUser;
            if (ownerUser == null) return "Undefined#0000";
            return $"{ownerUser.Username}#{ownerUser.Discriminator}";
        }

        public ulong GetOwnerID()
        {
            return EdgyBot.Credientals.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }

        /// <summary>
        /// Logs a LogMessage to the Console.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(DateTime.Now.ToShortTimeString() + " | " + message.Severity.ToString().ToUpper() + ": " + message.Message);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs a Message under the EdgyBot domain.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task EdgyLog(LogSeverity severity, string msg)
            => await Log(new LogMessage(severity, "EdgyBot", msg));
        
    }
}