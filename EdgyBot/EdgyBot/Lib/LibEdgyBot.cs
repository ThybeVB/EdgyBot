using Discord;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text;
using System.Security.Cryptography;

namespace EdgyBot
{
    /// <summary>
    /// Library for EdgyBot
    /// </summary>
    public class LibEdgyBot : ModuleBase<SocketCommandContext>
    {
        private readonly LoginInfo _loginInfo = new LoginInfo();

        public readonly Color LightBlue = new Color(0x0cc6d3);
        private readonly Color moneyGreen = new Color(0x85bb65);

        /// <summary>
        /// Creates an Embed with defaults and a field.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public Embed CreateEmbedWithText (string title, string text, bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }         
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
        public Embed CreateEmbedWithImage (string text, string imgUrl)
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
        public EmbedBuilder SetupEmbedWithDefaults (bool footerEnabled = false)
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
        /// Updates the Bots "Playing" message to the default EdgyBot status.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBotCountGameStatus ()
        {
            string gameStatus = "e!help | EdgyBot for " + Context.Client.Guilds.Count + " servers!";
            await Context.Client.SetGameAsync(gameStatus, type: ActivityType.Watching);
        }
        /// <summary>
        /// Encodes a string to Base 64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string EncodeB64(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            string result = System.Convert.ToBase64String(inputBytes);
            return result;
        }
        /// <summary>
        /// Decodes a string from Base 64
        /// </summary>
        /// <param name="encodedMessage"></param>
        /// <returns></returns>
        public string DecodeB64 (string encodedMessage)
        {
            byte[] inputBytes = Convert.FromBase64String(encodedMessage);
            string result = Encoding.UTF8.GetString(inputBytes);
            return result;
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
        /// <summary>
        /// Gets the Discord Bot ID
        /// </summary>
        /// <returns></returns>
        public ulong GetBotId ()
        {
            return Context.Client.CurrentUser.Id;
        }
        public string getDBLToken ()
        {
            string dblToken = _loginInfo.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }
        public string GetInviteLink ()
        {
            return _loginInfo.invLink;          
        }
        public string GetToken ()
        {
            return _loginInfo.token;
        }
        public string GetPrefix()
        {
            return _loginInfo.prefix;
        }
        public string GetGJP()
        {
            return _loginInfo.GJP;
        }
        public string GetGDAccID()
        {
            return _loginInfo.accID;
        }
        public string getOwnerDiscordName ()
        {
            return _loginInfo.ownerDiscordName;
        }
        public ulong GetOwnerID()
        {
            return _loginInfo.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }
        public int GetDefaultXP ()
        {
            return _loginInfo.startersXP;
        }
        public int GetDefaultAddedXP ()
        {
            return _loginInfo.defAddedXP;
        }
        public string GetDatabaseFileName ()
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Green;
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

        /// <summary>
        /// Creates an Embed exclusively for the Announce Command.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public Embed CreateAnnouncementEmbed(string msg, bool footerEnabled)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString() + " | If you want to stop getting these, use e!stopannounce.";
                eb.Footer = footer;
            }
            eb.Color = LightBlue;
            eb.AddField("ANNOUNCEMENT!", msg);
            return eb.Build();
        }
        public Embed CreateXPEmbed (string username, string currentXP)
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
