using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Discord;
using Discord.Commands;
using EdgyCore;
using EdgyCore.Lib;
using Discord.Addons.Interactive;

namespace EdgyBot.Modules
{
    public class HelpCommand : InteractiveBase<ShardedCommandContext>
    {
        public static CommandService _service;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly LibEdgyCore _core = new LibEdgyCore();

        public HelpCommand(CommandService service)
        {
            _service = service;
        }

        [Command("helpext")]
        public async Task HelpExperimental ()
        {
            var modulesAll = _service.Modules;
            var commandModules = modulesAll.Where(x => !string.IsNullOrEmpty(x.Summary));

            PaginatedMessage.Page[] pages = new PaginatedMessage.Page[commandModules.Count()];
            ModuleInfo[] modules = commandModules.ToArray();
            for (int x = 0; x <= (modules.Count() - 1); x++)
            {
                pages[x] = new PaginatedMessage.Page
                {
                    Title = modules[x].Name,
                    Description = modules[x].Summary,
                    Color = Color.Blue
                };
            }

            await PagedReplyAsync(new PaginatedMessage(pages));
        }

        [Command("help--text")]
        public async Task HelpCmdAlt1 ()
            => await HelpCmd("--text");

        [Command("help text")]
        public async Task HelpCmdAlt2()
            => await HelpCmd("--text");

        [Command("help", RunMode = RunMode.Async), Alias("commands")]
        public async Task HelpCmd ([Remainder]string param = "")
        {
            if (param != "--text")
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.AddField("Text Version", "To get a text version of all the commands, please add --text to the command.\nExample: e!help --text");
                eb.AddField("Web Version (Recommended)", "[EdgyBot Command List](http://edgybot.tk/commands.html)");
                eb.WithColor(0x0cc6d3);

                await ReplyAsync("", embed: eb.Build());
                await ReplyAsync("Support Server: https://discord.gg/hF4CSDG");

                return;
            }

            EmbedBuilder initEmbed = _lib.SetupEmbedWithDefaults();
            initEmbed.AddField("EdgyBot", "Help Command. Thanks for using EdgyBot!");
            initEmbed.AddField("Bot Prefix", _core.GetPrefix());
            initEmbed.AddField("help", "Shows this message!");
            initEmbed.AddField("command", "Gives info about a specific command.");
            await Context.User.SendMessageAsync("", embed: initEmbed.Build());

            await Context.Message.AddReactionAsync(new Emoji("📫"));

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
                await Task.Delay(TimeSpan.FromSeconds(1.25));
                await Context.User.SendMessageAsync("", embed: eb.Build());
            }
        }

        [Command("command"), Alias("cmdinfo", "cmd", "cmdhelp")]
        public async Task CommandInfoCmd (string cmdName = null) 
        {
            if (cmdName == "help" || cmdName == "command") {
                await ReplyAsync("This command does not have help.");
                return;
            }

            if (string.IsNullOrEmpty(cmdName)) {
                await ReplyAsync("Please enter a command name for help.");
                return;
            }

            CommandInfo command = _service.Commands.FirstOrDefault(x => x.Name == cmdName);
            
            if (command == null) {
                await ReplyAsync("This command does not exist. Did you mispell the command?");
                return;
            }

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            string aliasses = GetAliasString(command.Aliases);

            #region Footer
            if (string.IsNullOrEmpty(aliasses)) {
                eb.WithFooter(new EmbedFooterBuilder 
                {
                    Text = $"Command: {command.Name}, No Aliases."
                });
            } else 
            {
                eb.WithFooter(new EmbedFooterBuilder 
                {
                    Text = $"Command: {command.Name}, Aliases: {aliasses}"
                });
            }
            #endregion

            eb.AddField(command.Name, command.Summary);

            await ReplyAsync("", embed: eb.Build());
        }

        private string GetAliasString (IReadOnlyList<string> str) 
        {
            StringBuilder sb = new StringBuilder();

            foreach (string alias in str) 
            {
                if (string.IsNullOrEmpty(alias))
                    continue;

                sb.Append($"{alias} ");
            }
            return sb.ToString();
        }
    }
}
