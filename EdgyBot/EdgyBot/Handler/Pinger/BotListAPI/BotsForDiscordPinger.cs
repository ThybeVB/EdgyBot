using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace EdgyCore.Handler.Pinger
{
    public class BotsForDiscordPinger
    {
        JsonHelper helper = new JsonHelper("https://botsfordiscord.com/api/v1/bots/" + EdgyBot.Credientals.clientID);

        public async Task SendServerCountAsync ()
        {

        }
    }
}
