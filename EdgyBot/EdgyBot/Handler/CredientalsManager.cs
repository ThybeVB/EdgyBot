using EdgyCore;
using System;

namespace EdgyBot.Handler
{
    public class CredientalsManager
    {
        private string tokenEnv = "EdgyBot_Token";
        private string dblTokenEnv = "EdgyBot_DblToken";
        private string GJPEnv = "EdgyBot_GJP";

        public string prefixEnv = "EdgyBot_Prefix";
        public string clientIDEnv = "EdgyBot_ClientID";
        public string invLinkEnv = "EdgyBot_InvLink";
        public string accIDEnv = "EdgyBot_AccID";
        public string ownerIDEnv = "EdgyBot_OwnerID";

        public Credientals Read(bool firstTime)
        {
            if (firstTime)
            {
                ConsoleHelper();
            }

            return ReadVariables();
        }

        private void ConsoleHelper ()
        {
            #region Sensitive
            Console.WriteLine("EdgyBot Environment Variables, Make sure you get them right, manual edit.\nYou can leave fields empty, but note that some parts of the bot may not work.");

            Console.WriteLine("Bot Token");
            Environment.SetEnvironmentVariable(tokenEnv, Console.ReadLine());

            Console.WriteLine("Discord Bot List Token");
            Environment.SetEnvironmentVariable(dblTokenEnv, Console.ReadLine());

            Console.WriteLine("GJP");
            Environment.SetEnvironmentVariable(GJPEnv, Console.ReadLine());
            #endregion

            #region General
            Console.WriteLine("Bot Prefix");
            Environment.SetEnvironmentVariable(prefixEnv, Console.ReadLine());

            Console.WriteLine("Client ID");
            Environment.SetEnvironmentVariable(prefixEnv, Console.ReadLine());

            Console.WriteLine("Invite Link (Can be generated via Client ID)");
            Environment.SetEnvironmentVariable(invLinkEnv, Console.ReadLine());

            Console.WriteLine("Geometry Dash Account ID");
            Environment.SetEnvironmentVariable(GJPEnv, Console.ReadLine());

            Console.WriteLine("Owner ID");
            Environment.SetEnvironmentVariable(ownerIDEnv, Console.ReadLine());
            #endregion

            Environment.SetEnvironmentVariable("EdgyBot_IsSetup", "y");
        }

        private Credientals ReadVariables()
        {
            return null;
        }
    }
}
