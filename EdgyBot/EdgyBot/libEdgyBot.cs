using Discord;
using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace EdgyBot
{
    /// <summary>
    /// Library for EdgyBot
    /// </summary>
    public class LibEdgyBot : ModuleBase<SocketCommandContext>
    {
        private readonly LoginInfo _loginInfo = new LoginInfo();

        /// <summary>
        /// The default embed color for EdgyBot.
        /// </summary>
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
        /// Gets the bot's Invite Link
        /// </summary>
        /// <returns></returns>
        public string GetInviteLink ()
        {
            return _loginInfo.invLink;          
        }
        /// <summary>
        /// Gets the bot's token.
        /// </summary>
        /// <returns></returns>
        public string GetToken ()
        {
            return _loginInfo.token;
        }
        /// <summary>
        /// Gets the bot's prefix.
        /// </summary>
        /// <returns></returns>
        public string GetPrefix()
        {
            return _loginInfo.prefix;
        }
        /// <summary>
        /// Gets the Geometry Jump Password the bot is connected to.
        /// </summary>
        /// <returns></returns>
        public string GetGJP()
        {
            return _loginInfo.GJP;
        }
        /// <summary>
        /// Gets the Geometry Dash Account ID the bot is connected to.
        /// </summary>
        /// <returns></returns>
        public string GetGDAccID()
        {
            return _loginInfo.accID;
        }
        /// <summary>
        /// Get's the Discord User ID of the bot admin.
        /// </summary>
        /// <returns></returns>
        public ulong GetOwnerID()
        {
            return _loginInfo.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }
        /// <summary>
        /// The default XP value for every starting user.
        /// </summary>
        /// <returns></returns>
        public int GetDefaultXP ()
        {
            return _loginInfo.startersXP;
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
            eb.AddField("Username", username, true);
            eb.AddField("Experience", currentXP, true);
            
            return eb.Build();
        }
    }
}
