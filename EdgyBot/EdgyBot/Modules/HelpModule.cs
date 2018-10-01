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
                eb.AddField("Text Version", "To get a text (paged) version of all the commands, please add --text to the command.\nExample: e!help --text");
                eb.AddField("Web Version (Recommended)", "[EdgyBot Command List](https://edgybot.tk/commands.html)");
                eb.WithColor(0x0cc6d3);

                await ReplyAsync("", embed: eb.Build());
                await ReplyAsync("Support Server: https://discord.gg/hF4CSDG");

                return;
            }

            IEnumerable<ModuleInfo> commandModules = _service.Modules.Where(x => !string.IsNullOrEmpty(x.Summary));

            PaginatedMessage.Page[] pages = new PaginatedMessage.Page[commandModules.Count()];
            ModuleInfo[] modules = commandModules.ToArray();
            for (int x = 0; x <= (modules.Count() - 1); x++)
            {
                pages[x] = new PaginatedMessage.Page
                {
                    Title = modules[x].Name,
                    Description = modules[x].Summary,
                    Fields = GetFieldsForModule(modules[x]),
                    Color = Color.Blue,
                    ThumbnailUrl = "https://images-ext-2.discordapp.net/external/0A9ihJsopmMhAxVWOY4_kFEGwOxFgnAi0B1FTRSoQUU/%3Fsize%3D128/https/cdn.discordapp.com/avatars/373163613390897163/a6399df5d63b5fd8e42b446f75978407.png"
                };
            }

            await PagedReplyAsync(new PaginatedMessage(pages));
        }

        private List<EmbedFieldBuilder> GetFieldsForModule(ModuleInfo module)
        {
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();
            foreach (CommandInfo command in module.Commands)
            {
                if (command == null || command.Name == null || command.Summary == null)
                    continue;

                EmbedFieldBuilder field = new EmbedFieldBuilder
                {
                    Name = command.Name,
                    Value = command.Summary
                };

                fields.Add(field);
            }

            return fields;
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
