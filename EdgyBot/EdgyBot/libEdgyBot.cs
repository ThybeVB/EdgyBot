using Discord;
using System;
using System.Threading.Tasks;

namespace EdgyBot
{
    public class libEdgyBot
    {
        private LoginInfo loginInfo = new LoginInfo();

        public Color lightBlue = new Color(0x0cc6d3);

        public Embed createEmbedWithText (string title = null, string text = null)
        {
            if (title == null || text == null)
            {
                EmbedBuilder ebInv = new EmbedBuilder();
                ebInv.AddField("Error", "One or more parameters are missing.");
                ebInv.Color = lightBlue;
                Embed err = ebInv.Build();
                return err;
            }
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = lightBlue;
            eb.AddField(title, text);
            Embed e = eb.Build();

            return e;
        }
        public String getInviteLink ()
        {
            return loginInfo.invLink;          
        }
        public string getToken ()
        {
            return loginInfo.token;
        }
        public Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
