using Discord.Commands;
using EdgyBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdgyBot.Core
{
    public class LocalizationTest : ModuleBase<ShardedCommandContext>
    {
        private LocalizationService _service;

        public LocalizationTest (LocalizationService service)
        {
            _service = service;
        }

        [Command("l")]
        public async Task LCmd()
        {
            await ReplyAsync("");
        }
    }
}
