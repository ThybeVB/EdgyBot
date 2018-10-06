using System;
using EdgyBot.Core.Models;

namespace EdgyBot.Core.Handler
{
    public class CredentialsManager
    {
        #region Environment Variables
        private string tokenEnv = "EdgyBot_Token";
        private string dblTokenEnv = "EdgyBot_DblToken";
        private string dbTokenEnv = "EdgyBot_DbToken";
        private string bfdTokenEnv = "EdgyBot_BfdToken";
        private string blsTokenEnv = "EdgyBot_BlsToken";
        private string dblComTokenEnv = "EdgyBot_DblComToken";
        private string GJPEnv = "EdgyBot_GJP";

        private string prefixEnv = "EdgyBot_Prefix";
        private string clientIDEnv = "EdgyBot_ClientID";
        private string invLinkEnv = "EdgyBot_InvLink";
        private string accIDEnv = "EdgyBot_AccID";
        private string ownerIDEnv = "EdgyBot_OwnerID";
        #endregion

        public Credentials Read ()
        {
            return ReadVariables();
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
                dblComToken = Environment.GetEnvironmentVariable(dblComTokenEnv, EnvironmentVariableTarget.User),
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
