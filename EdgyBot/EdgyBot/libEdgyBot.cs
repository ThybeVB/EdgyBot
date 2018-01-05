using Discord;
using System;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class LibEdgyBot
    {
        private readonly LoginInfo loginInfo = new LoginInfo();

        private readonly Color lightBlue = new Color(0x0cc6d3);

        public Embed createEmbedWithText (string title, string text, bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }         
            eb.Color = lightBlue;
            eb.AddField(title, text);
            Embed e = eb.Build();

            return e;
        }
        public Embed createEmbedWithImage(string title, string text, string imgUrl)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = lightBlue;
            eb.AddField(title, text);
            eb.ImageUrl = imgUrl;
            Embed e = eb.Build();
            return e;
        }
        public EmbedBuilder setupEmbedWithDefaults (bool footerEnabled = false)
        {
            EmbedBuilder eb = new EmbedBuilder();
            if (footerEnabled)
            {
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.Text = DateTime.Now.ToUniversalTime().ToString();
                eb.Footer = footer;
            }
            eb.Color = lightBlue;
            return eb;
        }
        public string getInviteLink ()
        {
            return loginInfo.invLink;          
        }
        public string getToken ()
        {
            return loginInfo.token;
        }
        public string getPrefix()
        {
            return loginInfo.prefix;
        }
        public string getGJP()
        {
            return loginInfo.GJP;
        }
        public string getGDAccID()
        {
            return loginInfo.accID;
        }      
        public Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Info:
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
                    break;
            }
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        public Task eLog(string msg)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Verbose, "EdgyBot", msg);
            Console.WriteLine(logMessage.ToString());
            return Task.CompletedTask;
        }
    }
}
