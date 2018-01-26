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
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public HelpCommand(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        [Name("help")]
        [Summary("Gives EdgyBot's commands and what they do.")]
        public async Task HelpCmd ()
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();

            foreach (CommandInfo c in _service.Commands)
            {
                if (c == null) continue;
                try
                {
                    eb.AddField(c.Name, c.Summary);
                }
                catch
                {
                    await _lib.EdgyLog(LogSeverity.Warning, "Could not add " + c.Name + " to the Help command.");
                }
            }

            Embed e = eb.Build();
            await dm.SendMessageAsync("", embed: e);
            await Context.Message.AddReactionAsync(new Emoji("📫"));
        }
    }
}
