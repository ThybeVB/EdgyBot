using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EdgyBot.Modules
{
    public class XPCommands : ModuleBase<SocketCommandContext>
    {
        LoginInfo loginInfo = new LoginInfo();
        libEdgyBot lib = new libEdgyBot();

        [Command("xp")]
        public async Task XPGetCMD(IGuildUser usr)
        {
            string inputUserID = usr.Id.ToString();
        }
    }
}
