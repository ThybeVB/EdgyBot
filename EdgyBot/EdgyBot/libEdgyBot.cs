using Discord;
using System;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class LibEdgyBot
    {
        private LoginInfo loginInfo = new LoginInfo();

        public Color lightBlue = new Color(0x0cc6d3);

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
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
