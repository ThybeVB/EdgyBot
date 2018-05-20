using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EdgyCore.Modules
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public HelpCommand(CommandService service)
        {
            _service = service;
        }

        [Command("help", RunMode = RunMode.Async)]
        public async Task HelpCmd ()
        {
            await Context.Message.AddReactionAsync(new Emoji("📫"));

            EmbedBuilder initEmbed = LibEdgyBot.Instance.SetupEmbedWithDefaults();
            initEmbed.AddField("EdgyBot", "Help Command. Thanks for using EdgyBot!");
            initEmbed.AddField("Bot Prefix", LibEdgyBot.Instance.GetPrefix());
            await Context.User.SendMessageAsync("", embed: initEmbed.Build());

            foreach (ModuleInfo module in _service.Modules)
            {
                if (module == null || string.IsNullOrEmpty(module.Name) || string.IsNullOrEmpty(module.Summary))
                    continue;

                EmbedBuilder eb = LibEdgyBot.Instance.SetupEmbedWithDefaults();
                eb.AddField(module.Name, module.Summary);

                foreach (CommandInfo command in module.Commands)
                {
                    if (command == null || command.Name == null || command.Summary == null)
                        continue;
                    eb.AddField(command.Name, command.Summary);
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
                await Context.User.SendMessageAsync("", embed: eb.Build());
            }
        }
    }
}
