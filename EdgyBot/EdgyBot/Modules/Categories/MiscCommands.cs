using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System;

namespace EdgyCore.Modules
{
    public class MiscCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly Database _database = new Database();

        [Command("invite")][Name("invite")][Summary("Gives the Invite Link for EdgyBot.")]
        public async Task InviteCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Invite Link", _lib.GetInviteLink() + "\nThanks for inviting EdgyBot, you're awesome :)");
            await ReplyAsync("", embed: e);
        }
        [Command("source")][Alias("sourcecode")][Name("source")][Summary("Links the Source Code of EdgyBot")]
        public async Task SourceCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Source Code", "https://github.com/MonstahGames/EdgyBot \nIf you have a GitHub account, be sure to :star: the Repository!");
            await ReplyAsync("", embed: e);
        }
        [Command("say")][Name("say")][Summary("Have EdgyBot send your message.")]
        public async Task SayCmd ([Remainder]string input = null)
        {
            await ReplyAsync(input);
        }
        [Command("sayd")][Name("sayd")][Alias("saydelete")][Summary("This does the same thing as 'sayd', but deletes the original message.")]
        public async Task SaydCmd ([Remainder]string input = null)
        {
            await ReplyAsync(input);
            try { await Context.Message.DeleteAsync(); } catch
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError("Say Delete Command", "Could not delete the message. Does the bot have the permissions to do so?"));
            }      
        }    
        [Command("e")]
        public async Task SecretECmd()
        {
            await ReplyAsync("monstah is not gay german");
        }
        /* Marking suggest command as async because it was throwing MessageRecieved errors. */
        [Command("suggest", RunMode = RunMode.Async)][Alias("sg", "sugg")][Name("suggest")][Summary("Sends your suggestion to the owner of the bot.")]
        public async Task SuggestCmd ([Remainder]string msg = null)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("New Suggestion!", msg);
            #region Footer
            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            efb.Text = Context.User.Username + $"#{Context.User.Discriminator}" + " in " + Context.Guild.Name + " at " + DateTime.Now.ToLongTimeString();
            eb.Footer = efb;
            #endregion
            await EventHandler.OwnerUser.SendMessageAsync("", embed: eb.Build());
            await ReplyAsync("Your message has been sent!");
        }
        [Command("support")][Name("support")][Summary("Tells you how you can support the development of EdgyBot")]
        public async Task SupportCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Discord Bot List", "Upvoting EdgyBot on the Discord Bot List helps EdgyBot get into more servers for even more fun.");
            eb.AddField("Bot List Link", "https://discordbots.org/bot/373163613390897163");
            eb.AddField("Moneys!11!", "Having a concurrent earning of a bit under 10$, I can keep EdgyBot running 24/7");
            eb.AddField("Support Link", "https://www.paypal.me/monstahhh");          
            eb.WithFooter(new EmbedFooterBuilder{Text = "Thanks for taking your time to even look at this command. People like you keep EdgyBot running."});
            await ReplyAsync("", embed: eb.Build());
        }
    }
}