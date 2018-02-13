using System.Threading.Tasks;
using Discord;
using Discord.Commands;

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
        [Command("help", RunMode = RunMode.Async)]
        [Name("help")]
        [Summary("Gives EdgyBot's commands and what they do.")]
        public async Task HelpCmd()
        {
            IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();
        
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            EmbedBuilder eb2 = _lib.SetupEmbedWithDefaults();
        
            eb.AddField("Bot Prefix", _lib.GetPrefix());
        
            int lineCount = 0;
            foreach (CommandInfo c in _service.Commands)
            {
                if (c == null) continue;
                if (c.Name == null || c.Summary == null) continue;
                if (lineCount >= 24)
                {
                    eb2.AddField(c.Name, c.Summary);
                    lineCount++;
                    continue;
                }
                eb.AddField(c.Name, c.Summary);
                lineCount++;
            }
            Embed e = eb.Build();
            Embed e2 = eb2.Build();
            await dm.SendMessageAsync("", embed: e);
            await dm.SendMessageAsync("", embed: e2);
            await Context.Message.AddReactionAsync(new Emoji("📫"));
        }
    }
}
