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

        public readonly Color _lightBlue = new Color(0x0cc6d3);

        /// <summary>
        /// Creates an Embed with defaults and a field.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public Embed createEmbedWithText (string title, string text, bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }         
            eb.Color = _lightBlue;
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
        public Embed createEmbedWithImage (string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = _lightBlue;
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
        public Embed createEmbedWithImage(string title, string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = _lightBlue;
            eb.AddField(title, text);
            eb.ImageUrl = imgUrl;
            Embed e = eb.Build();
            return e;
        }
        public Embed createEmbedWithError(string errTitle, string errText)
        {
            return null;
        }
        /// <summary>
        /// Creates an Embed with defaults.
        /// </summary>
        /// <param name="footerEnabled"></param>
        /// <returns></returns>
        public EmbedBuilder setupEmbedWithDefaults (bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }
            eb.Color = _lightBlue;
            return eb;
        }
        /// <summary>
        /// Gets the bot's Invite Link
        /// </summary>
        /// <returns></returns>
        public string getInviteLink ()
        {
            return _loginInfo.invLink;          
        }
        /// <summary>
        /// Gets the bot's token.
        /// </summary>
        /// <returns></returns>
        public string getToken ()
        {
            return _loginInfo.token;
        }
        /// <summary>
        /// Gets the bot's prefix.
        /// </summary>
        /// <returns></returns>
        public string getPrefix()
        {
            return _loginInfo.prefix;
        }
        /// <summary>
        /// Gets the Geometry Jump Password the bot is connected to.
        /// </summary>
        /// <returns></returns>
        public string getGJP()
        {
            return _loginInfo.GJP;
        }
        /// <summary>
        /// Gets the Geometry Dash Account ID the bot is connected to.
        /// </summary>
        /// <returns></returns>
        public string getGDAccID()
        {
            return _loginInfo.accID;
        }
        /// <summary>
        /// Get's the Discord User ID of the bot admin.
        /// </summary>
        /// <returns></returns>
        public ulong getOwnerID()
        {
            return _loginInfo.ownerID;
        }
        public string getProfilePicUrl()
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

            string messageStr = message.ToString();
            Console.WriteLine(messageStr);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs a Message under the EdgyBot name.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task edgyLog(LogSeverity severity, string msg)
        {
            LogMessage logMessage = new LogMessage(severity, "EdgyBot", msg);
            Log(logMessage);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates an Embed exclusively for the Announce Command.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Embed createAnnouncementEmbed(string msg, bool footerEnabled)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }
            eb.Color = _lightBlue;
            eb.AddField("ANNOUNCEMENT!", msg);
            return eb.Build();
        }
    }
}
