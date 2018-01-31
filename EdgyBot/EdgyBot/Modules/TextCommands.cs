using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace EdgyBot.Modules
{
    public class TextCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly Database _database = new Database();

        [Command("invite")][Name("invite")][Summary("Gives the Invite Link for EdgyBot.")]
        public async Task InviteCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Invite Link", _lib.GetInviteLink() + "\nThanks for inviting EdgyBot, you're awesome :)");
            await ReplyAsync("", embed: e);
        }
        [Command("source")][Alias("sourcecode"][Name("source")][Summary("Links the Source Code of EdgyBot")]
        public async Task SourceCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Source Code", "https://github.com/MonstahGames/EdgyBot");
            await ReplyAsync("", embed: e);
        }
        [Command("say")][Name("say")][Summary("Have EdgyBot send your message.")]
        public async Task SayCmd ([Remainder]string input = null)
        {
            if (input == null)
            {
                await ReplyAsync("Please enter a message.\nDon't forget to use **Quotation Marks**!");
                return;
            }
            await ReplyAsync(input);
        }
        [Command("sayd")][Name("sayd")][Alias("saydelete")][Summary("This does the same thing as 'sayd', but deletes the original message.")]
        public async Task SaydCmd ([Remainder]string input = null)
        {
            if (input == null)
            {
                Embed e = _lib.CreateEmbedWithText("Sayd", "Please enter a message.\nDon't forget to use **Quotation Marks**!", true);
                await ReplyAsync("", embed: e);
            } else
            {
                await ReplyAsync(input);
                await Context.Message.DeleteAsync();
            }         
        }    
        [Command("e")]
        public async Task Secret01Cmd()
        {
            await ReplyAsync("monstah is not gay german");
        }
        [Command("suggest")][Alias("sg")][Name("suggest")][Summary("Sends your suggestion to the owner of the bot.")]
        public async Task SuggestCmd ([Remainder]string msg = null)
        {
            if (msg != null)
            {
                await EventHandler.OwnerUser.SendMessageAsync(msg);
                await ReplyAsync("Your message has been sent!");
            }
            else
            {
                await ReplyAsync("Please enter a message.");
            }      
        }
    }
}