using System;
using EdgyCore;
using EdgyCore.Common;

namespace EdgyCore.Handler
{
    public class CredientalsManager
    {
        #region Environment Variables
        private string tokenEnv = "EdgyBot_Token";
        private string dblTokenEnv = "EdgyBot_DblToken";
        private string dbTokenEnv = "EdgyBot_DbToken";
        private string bfdTokenEnv = "EdgyBot_BfdToken";
        private string blsTokenEnv = "EdgyBot_BlsToken";
        private string GJPEnv = "EdgyBot_GJP";

        public string prefixEnv = "EdgyBot_Prefix";
        public string clientIDEnv = "EdgyBot_ClientID";
        public string invLinkEnv = "EdgyBot_InvLink";
        public string accIDEnv = "EdgyBot_AccID";
        public string ownerIDEnv = "EdgyBot_OwnerID";
        #endregion

        public Credentials Read(bool firstTime)
        {
            if (firstTime)
                ConsoleHelper();
            
            return ReadVariables();
        }

        private void ConsoleHelper ()
        {
            #region Sensitive
            Console.WriteLine("EdgyBot Environment Variables, Make sure you get them right, manual edit.\nYou can leave fields empty, but note that some parts of the bot may not work.");

            Console.WriteLine("Bot Token");
            Environment.SetEnvironmentVariable(tokenEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Discord PW Bot List Token");
            Environment.SetEnvironmentVariable(dbTokenEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Discord Bot List Token");
            Environment.SetEnvironmentVariable(dblTokenEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Bots For Discord Token");
            Environment.SetEnvironmentVariable(bfdTokenEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Discord Botlist Space Token");
            Environment.SetEnvironmentVariable(blsTokenEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("GJP");
            Environment.SetEnvironmentVariable(GJPEnv, Console.ReadLine(), EnvironmentVariableTarget.User);
            #endregion

            #region General
            Console.WriteLine("Bot Prefix");
            Environment.SetEnvironmentVariable(prefixEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Client ID");
            Environment.SetEnvironmentVariable(clientIDEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Invite Link (Can be generated via Client ID)");
            Environment.SetEnvironmentVariable(invLinkEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Geometry Dash Account ID");
            Environment.SetEnvironmentVariable(accIDEnv, Console.ReadLine(), EnvironmentVariableTarget.User);

            Console.WriteLine("Owner ID");
            Environment.SetEnvironmentVariable(ownerIDEnv, Console.ReadLine(), EnvironmentVariableTarget.User);
            #endregion

            Environment.SetEnvironmentVariable("EdgyBot_IsSetup", "n", EnvironmentVariableTarget.User);
        }

        private Credentials ReadVariables()
        {
            string clientIDStr = Environment.GetEnvironmentVariable(clientIDEnv, EnvironmentVariableTarget.User);
            ulong clientID = Convert.ToUInt64(clientIDStr);

            string ownerIDStr = Environment.GetEnvironmentVariable(ownerIDEnv, EnvironmentVariableTarget.User);
            ulong ownerID = Convert.ToUInt64(ownerIDStr);

            Credentials creds = new Credentials
            {
                token = Environment.GetEnvironmentVariable(tokenEnv, EnvironmentVariableTarget.User),
                dblToken = Environment.GetEnvironmentVariable(dblTokenEnv, EnvironmentVariableTarget.User),
                dbToken = Environment.GetEnvironmentVariable(dbTokenEnv, EnvironmentVariableTarget.User),
                bfdToken = Environment.GetEnvironmentVariable(bfdTokenEnv, EnvironmentVariableTarget.User),
                blsToken = Environment.GetEnvironmentVariable(blsTokenEnv, EnvironmentVariableTarget.User),
                GJP = Environment.GetEnvironmentVariable(GJPEnv, EnvironmentVariableTarget.User),
                prefix = Environment.GetEnvironmentVariable(prefixEnv, EnvironmentVariableTarget.User),
                clientID = clientID,
                invLink = Environment.GetEnvironmentVariable(invLinkEnv, EnvironmentVariableTarget.User),
                accID = Environment.GetEnvironmentVariable(accIDEnv, EnvironmentVariableTarget.User),
                ownerID = ownerID
            };

            return creds;
        }
    }
}
