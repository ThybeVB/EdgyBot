using System;
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
        public async Task NewHelpCmd ()
        {
            await Context.Message.AddReactionAsync(new Emoji("📫"));

            EmbedBuilder initEmbed = _lib.SetupEmbedWithDefaults();
            initEmbed.AddField("EdgyBot", "Help Command. Thanks for using EdgyBot!");
            initEmbed.AddField("Bot Prefix", _lib.GetPrefix());
            await Context.User.SendMessageAsync("", embed: initEmbed.Build());

            foreach (ModuleInfo module in _service.Modules)
            {
                if (module == null || string.IsNullOrEmpty(module.Name) || string.IsNullOrEmpty(module.Summary))
                    continue;

                EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
                eb.AddField(module.Name, module.Summary);

                foreach (CommandInfo command in module.Commands)
                {
                    if (command == null || command.Name == null || command.Summary == null)
                        continue;
                    eb.AddField(command.Name, command.Summary);
                }

                //await Task.Delay(TimeSpan.FromSeconds(1));
                await Task.Delay(TimeSpan.FromSeconds(1.75));
                await Context.User.SendMessageAsync("", embed: eb.Build());
            }
        }

       //[Command("help", RunMode = RunMode.Async)]
       //[Name("help")]
       //[Summary("Gives EdgyBot's commands and what they do.")]
       //public async Task HelpCmd()
       //{
       //    IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync();
       //
       //    EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
       //    EmbedBuilder eb2 = _lib.SetupEmbedWithDefaults();
       //
       //    eb.AddField("Bot Prefix", _lib.GetPrefix());
       //    eb.AddField("Image Commands", "Please check the 'imagecommands' Command for some needed info.");
       //
       //    int lineCount = 0;
       //    foreach (CommandInfo c in _service.Commands)
       //    {
       //        if (c == null) continue;
       //        if (c.Name == null || c.Summary == null) continue;
       //        if (lineCount >= 23)
       //        {
       //            eb2.AddField(c.Name, c.Summary);
       //            lineCount++;
       //            continue;
       //        }
       //        eb.AddField(c.Name, c.Summary);
       //        lineCount++;
       //    }
       //    await dm.SendMessageAsync("", embed: eb.Build());
       //    await dm.SendMessageAsync("", embed: eb2.Build());
       //
       //    await Context.Message.AddReactionAsync(new Emoji("📫"));
       //}
    }
}
