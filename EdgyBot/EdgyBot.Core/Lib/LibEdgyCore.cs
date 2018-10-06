using System;
using EdgyBot.Core.Models;
using EdgyBot.Core.Handler;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace EdgyBot.Core.Lib
{
    public class LibEdgyCore : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public ulong GetBotId()
        {
            return Bot.Credentials.clientID;
        }

        public string GetDBLToken()
        {
            string dblToken = Bot.Credentials.dblToken;
            if (string.IsNullOrEmpty(dblToken)) return null;
            return dblToken;
        }

        public string GetInviteLink()
        {
            string invLink = Bot.Credentials.invLink;
            if (string.IsNullOrEmpty(invLink))
            {
                string clientID = GetClientID().ToString();
                invLink = "https://discordapp.com/oauth2/authorize/?permissions=2146950391&scope=bot&client_id=" + clientID;
            }
            return invLink;
        }

        public string GetToken()
        {
            string token = Bot.Credentials.token;
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("EdgyBot Token", "EdgyBot Token is invalid or not set.");
            }
            return token;
        }

        public string GetPrefix()
        {
            string prefix = Bot.Credentials.prefix;

            if (string.IsNullOrEmpty(prefix)) return "e!";
            return prefix;
        }

        public string GetGJP()
        {
            string gjp = Bot.Credentials.GJP;
            if (string.IsNullOrEmpty(gjp)) return null;
            return gjp;
        }

        public ulong GetClientID()
        {
            ulong clientID = Bot.Credentials.clientID;
            return clientID;
        }

        public string GetGDAccID()
        {
            string accID = Bot.Credentials.accID;
            if (string.IsNullOrEmpty(accID))
            {
                _lib.Log(new LogMessage(LogSeverity.Error, "EdgyBot", "GD Account ID Error"));
                return null;
            }
            return Bot.Credentials.accID;
        }

        public string GetOwnerDiscordName()
        {
            SocketUser ownerUser = Handler.EventHandler.OwnerUser;
            if (ownerUser == null) return "Undefined#0000";
            return $"{ownerUser.Username}#{ownerUser.Discriminator}";
        }

        public ulong GetOwnerID()
        {
            return Bot.Credentials.ownerID;
        }
        public string GetProfilePicUrl()
        {
            return Context.Client.CurrentUser.GetAvatarUrl();
        }
    }
}
