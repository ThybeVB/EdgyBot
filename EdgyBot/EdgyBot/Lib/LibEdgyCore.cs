using System;
using EdgyCore.Common;
using EdgyCore.Handler;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

namespace EdgyCore.Lib
{
    public class LibEdgyCore : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public Credentials GetCredentials ()
        {
            CredientalsManager manager = new CredientalsManager();

            string isSetup = Environment.GetEnvironmentVariable("EdgyBot_IsSetup", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(isSetup) || isSetup == "y")
            {
                return manager.Read(true);
            } else
            {
                return manager.Read(false);
            }
        }

        public ulong GetBotId()
        {
            return EdgyBot.Credentials.clientID;
        }

        public string GetDBLToken()
        {
            string dblToken = EdgyBot.Credentials.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }

        public string GetInviteLink()
        {
            string invLink = EdgyBot.Credentials.invLink;
            if (string.IsNullOrEmpty(invLink))
            {
                string clientID = GetClientID().ToString();
                invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=" + clientID;
            }
            return invLink;
        }

        public string GetToken()
        {
            string token = EdgyBot.Credentials.token;
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("EdgyBot Token", "EdgyBot Token is invalid or not set.");
            }
            return token;
        }

        public string GetPrefix()
        {
            string prefix = EdgyBot.Credentials.prefix;

            if (string.IsNullOrEmpty(prefix)) return "e!";
            return prefix;
        }

        public string GetGJP()
        {
            string gjp = EdgyBot.Credentials.GJP;
            if (string.IsNullOrEmpty(gjp)) return null;
            return gjp;
        }

        public ulong GetClientID()
        {
            ulong clientID = EdgyBot.Credentials.clientID;
            return clientID;
        }

        public string GetGDAccID()
        {
            string accID = EdgyBot.Credentials.accID;
            if (string.IsNullOrEmpty(accID))
            {
                _lib.Log(new LogMessage(LogSeverity.Error, "EdgyBot", "GD Account ID Error"));
                return null;
            }
            return EdgyBot.Credentials.accID;
        }

        public string GetOwnerDiscordName()
        {
            SocketUser ownerUser = Handler.EventHandler.OwnerUser;
            if (ownerUser == null) return "Undefined#0000";
            return $"{ownerUser.Username}#{ownerUser.Discriminator}";
        }

        public ulong GetOwnerID()
        {
            return EdgyBot.Credentials.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }
    }
}
