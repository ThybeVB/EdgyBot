using Discord;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Discord.WebSocket;
using System.Net;

namespace EdgyCore
{
    /// <summary>
    /// Library for EdgyBot
    /// </summary>
    public class LibEdgyBot : ModuleBase<SocketCommandContext>
    {
        private readonly LoginInfo _loginInfo = new LoginInfo();

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
            Embed e = eb.Build();

            return e;
        }

        /// <summary>
        /// Creates an Embed with Text and an image.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithImage(string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = LightBlue;
            eb.ImageUrl = imgUrl;
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
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = LightBlue;
            eb.AddField(title, text);
            eb.WithImageUrl(imgUrl);
            Embed e = eb.Build();
            return e;
        }

        /// <summary>
        /// Creates an Error Embed.
        /// </summary>
        /// <param name="errTitle"></param>
        /// <param name="errText"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithError(string errTitle, string errText)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = Color.DarkRed;
            eb.AddField(errTitle, errText);
            Embed e = eb.Build();
            return e;
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
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
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
            return Context.Client.CurrentUser.Id;
        }

        public string getDBLToken()
        {
            string dblToken = _loginInfo.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }

        public string GetInviteLink()
        {
            return _loginInfo.invLink;
        }

        public string GetToken()
        {
            return _loginInfo.token;
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
            return _loginInfo.prefix;
        }

        public string GetGJP()
        {
            string gjp = _loginInfo.GJP;
            if (string.IsNullOrEmpty(gjp)) return null;
            return gjp;
        }

        public string GetGDAccID()
        {
            string accID = _loginInfo.accID;
            if (string.IsNullOrEmpty(accID)) return null;
            return _loginInfo.accID;
        }

        public string getOwnerDiscordName()
        {
            SocketUser ownerUser = EventHandler.OwnerUser;
            if (ownerUser == null) return "Undefined";
            return $"{ownerUser.Username}#{ownerUser.Discriminator}";
        }

        public ulong GetOwnerID()
        {
            return _loginInfo.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }

        public string GetDatabaseFileName()
        {
            return _loginInfo.dbname;
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
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs a Message under the EdgyBot name.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task EdgyLog(LogSeverity severity, string msg)
        {
            LogMessage logMessage = new LogMessage(severity, "EdgyBot", msg);
            Log(logMessage);
            return Task.CompletedTask;
        }

        public Embed CreateXPEmbed(string username, string currentXP)
        {

            EmbedBuilder eb = new EmbedBuilder();
            eb.Title = "EdgyBot Experience System";
            eb.Color = moneyGreen;
            eb.ThumbnailUrl = "https://www.wpclipart.com/money/US_Currency/US_hundred_dollar_bill.png";
            eb.AddField("Username", username, true);
            eb.AddField("Experience", currentXP, true);

            return eb.Build();
        }
    }
}