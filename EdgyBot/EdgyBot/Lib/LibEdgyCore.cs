using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using EdgyCore.Handler;

namespace EdgyCore.Lib
{
    public class LibEdgyCore : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public Credentials GetCredientals ()
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
            return EdgyBot.Credientals.clientID;
        }

        public string GetDBLToken()
        {
            string dblToken = EdgyBot.Credientals.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }

        public string GetInviteLink()
        {
            string invLink = EdgyBot.Credientals.invLink;
            if (string.IsNullOrEmpty(invLink))
            {
                string clientID = GetClientID().ToString();
                invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=" + clientID;
            }
            return invLink;
        }

        public string GetToken()
        {
            string token = EdgyBot.Credientals.token;
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("EdgyBot Token", "EdgyBot Token is invalid or not set.");
            }
            return token;
        }

        public string GetPrefix()
        {
            string prefix = EdgyBot.Credientals.prefix;

            if (string.IsNullOrEmpty(prefix)) return "e!";
            return prefix;
        }

        public string GetGJP()
        {
            string gjp = EdgyBot.Credientals.GJP;
            if (string.IsNullOrEmpty(gjp)) return null;
            return gjp;
        }

        public ulong GetClientID()
        {
            ulong clientID = EdgyBot.Credientals.clientID;
            return clientID;
        }

        public string GetGDAccID()
        {
            string accID = EdgyBot.Credientals.accID;
            if (string.IsNullOrEmpty(accID))
            {
                _lib.Log(new LogMessage(LogSeverity.Error, "EdgyBot", "GD Account ID Error"));
                return null;
            }
            return EdgyBot.Credientals.accID;
        }

        public string GetOwnerDiscordName()
        {
            SocketUser ownerUser = Handler.EventHandler.OwnerUser;
            if (ownerUser == null) return "Undefined#0000";
            return $"{ownerUser.Username}#{ownerUser.Discriminator}";
        }

        public ulong GetOwnerID()
        {
            return EdgyBot.Credientals.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }
    }
}
