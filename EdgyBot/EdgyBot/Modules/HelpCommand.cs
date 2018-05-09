using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EdgyCore.Modules
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public HelpCommand(CommandService service)
        {
            _service = service;
        }

        [Command("help", RunMode = RunMode.Async)]
        [Name("help")]
        [Summary("Gives EdgyBot's commands and what they do.")]
        public async Task HelpCmd()
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();
        
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            EmbedBuilder eb2 = _lib.SetupEmbedWithDefaults();
        
            eb.AddField("Bot Prefix", _lib.GetPrefix());
            eb.AddField("Image Commands", "Please check the 'imagecommands' Command for some needed info.");
        
            int lineCount = 0;
            foreach (CommandInfo c in _service.Commands)
            {
                if (c == null) continue;
                if (c.Name == null || c.Summary == null) continue;
                if (lineCount >= 23)
                {
                    eb2.AddField(c.Name, c.Summary);
                    lineCount++;
                    continue;
                }
                eb.AddField(c.Name, c.Summary);
                lineCount++;
            }
            await dm.SendMessageAsync("", embed: eb.Build());
            await dm.SendMessageAsync("", embed: eb2.Build());

            await Context.Message.AddReactionAsync(new Emoji("📫"));
        }
    }
}
