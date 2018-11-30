using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using EdgyBot.Core;
using EdgyBot.Core.Lib;

namespace EdgyBot.Modules
{
    [Name("Miscellanious Commands"), Summary("Commands that don't fit in any other categories.")]
    public class MiscCommands : ModuleBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private LibEdgyCore _core = new LibEdgyCore();

        [Command("invite")] [Name("invite")] [Summary("Gives the Invite Link for EdgyBot.")]
        public async Task InviteCmd()
        {
            Embed e = _lib.CreateEmbedWithText((string)Context.Language["misc"]["invLink"], _core.GetInviteLink() + Context.Language["misc"]["thanksInvite"]);
            await ReplyAsync("", embed: e);
        }

        [Command("source"), Alias("sourcecode", "github")]
        [Name("source")] [Summary("Links the Source Code of EdgyBot")]
        public async Task SourceCmd()
            => await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["misc"]["src"], (string)Context.Language["misc"]["git"]));

        [Command("say")] [Name("say")] [Summary("Have EdgyBot send your message.")]
        public async Task SayCmd([Remainder]string input)
            => await ReplyAsync(input);

        [Command("sayd")] [Name("sayd")] [Alias("saydelete")] [Summary("This does the same thing as 'sayd', but deletes the original message.")]
        public async Task SaydCmd([Remainder]string input)
        {
            await ReplyAsync(input);
            try { await Context.Message.DeleteAsync(); } catch
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError((string)Context.Language["misc"]["sayd"], (string)Context.Language["misc"]["permFail"]));
            }
        }

        [Command("e")]
        public async Task ECmd()
        {
            //This is just a fun little inside joke, That's why it's only meant for one server.
            if (Context.Guild.Id != 424929039237775361)
                return;

            await ReplyAsync("monstah is not gay german");
        }

        [Command("suggest", RunMode = RunMode.Async)] [Alias("sg", "sugg")] [Name("suggest")] [Summary("Sends your suggestion to the owner of the bot.")]
        public async Task SuggestCmd([Remainder]string msg = null)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("New Suggestion!", msg);
            EmbedFooterBuilder efb = new EmbedFooterBuilder
            {
                Text = Context.User.Username + $"#{Context.User.Discriminator}" + " in " + Context.Guild.Name + " at " + DateTime.Now.ToLongTimeString()
            };
            eb.Footer = efb;
            await EdgyBot.Core.Handler.EventHandler.OwnerUser.SendMessageAsync("", embed: eb.Build());
            await ReplyAsync((string)Context.Language["misc"]["msgSent"]);
        }

        [Command("bugreport")] [Alias("reportbug")] [Name("bugreport")] [Summary("Submits a bug to the owner of the bot. Please use this command wisely. The command, steps to reproduce, etc...")]
        public async Task BugReportCmd([Remainder]string msg = null)
        {
            if (msg == null)
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError((string)Context.Language["misc"]["bugErr"], (string)Context.Language["misc"]["enterMsg"]));
                return;
            }
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("New Bug!", msg);
            EmbedFooterBuilder efb = new EmbedFooterBuilder
            {
                Text = Context.User.Username + $"#{Context.User.Discriminator}" + " in " + Context.Guild.Name + " at " + DateTime.Now.ToLongTimeString()
            };
            eb.Footer = efb;
            await EdgyBot.Core.Handler.EventHandler.OwnerUser.SendMessageAsync("", embed: eb.Build());
            await ReplyAsync("", embed: _lib.CreateEmbedWithText((string)Context.Language["misc"]["succ"], (string)Context.Language["misc"]["bugSent"]));
        }
    }
}