using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyCore;

namespace EdgyBot.Modules.Categories
{
    [Name("Fun Commands"), Summary("Commands used for FUNUNUNUN")]
    public class FunCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("duel", RunMode = RunMode.Async)][Name("duel")][Summary("Duels a user")]
        public async Task DuelCmd (SocketGuildUser usr = null)
        {
            Random rand = new Random();

            if (usr == null || usr == Context.Message.Author) {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError("Duel Command", "Please mention a user!"));
                return;
            }
            IUserMessage msg;
            msg = await Context.Channel.SendMessageAsync(":crossed_swords: Dueling " + usr.Username + ".");
            await Task.Delay(TimeSpan.FromSeconds(1.0));

            await msg.ModifyAsync(x => x.Content = ":crossed_swords: Dueling " + usr.Username + "..");
            await Task.Delay(TimeSpan.FromSeconds(1.0));

            await msg.ModifyAsync(x => x.Content = ":crossed_swords: Dueling " + usr.Username + "...");

            switch (rand.Next(-1, 2))
            {
                default:
                    await msg.ModifyAsync(x => x.Content = Context.Message.Author.Mention + " has won!");
                    break;
                case 0:
                    await msg.ModifyAsync(x => x.Content = Context.Message.Author.Mention + " has won!");
                    break;
                case 1:
                    await msg.ModifyAsync(x => x.Content = usr.Mention + " has won!");
                    break;
            }
        }

        [Command("clap")][Name("clap")][Summary("Puts your message into claps")]
        public async Task ClapCmd ([Remainder]string input)
        {
            char[] characters = input.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char character in characters)
            {
                if (character == ' ')
                {
                    sb.Append(":clap:");
                }
                sb.Append(character);
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("vertical")][Name("vertical")][Summary("Converts your message to a vertical one")]
        public async Task VerticalCmd([Remainder]string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("```" + "\n");
            char[] msgArray = msg.ToCharArray();
            foreach (char letter in msgArray)
            {
                sb.Append(letter + "\n");
            }
            sb.Append("```" + "\n");
            await ReplyAsync(sb.ToString());
        }

        [Command("bigletter")][Name("bigletter")][Summary("Converts your message to Big Letters")]
        public async Task BigLetterCmd([Remainder]string msg)
        {
            msg = msg.ToLower();
            StringBuilder sb = new StringBuilder();
            char[] letters = msg.ToCharArray();
            foreach (char letter in letters)
            {
                if (!_lib.IsEnglishLetter(letter)) continue;
                if (letter == ' ')
                {
                    sb.Append(" ");
                    continue;
                }
                sb.Append(":regional_indicator_" + letter + ":");
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("stop")][Name("stop")][Summary("Tells somebody to **STOP**")]
        public async Task StopCmd(IGuildUser usr)
        {
            string stopUrl = "https://i.imgur.com/1TdHj1y.gif";
            Embed a = _lib.CreateEmbedWithImage(usr.Mention, stopUrl);
            await ReplyAsync("", embed: a);
        }

        [Command("jeff")][Name("jeff")][Summary("Jeff's somebody.")]
        public async Task JeffCmd(IGuildUser user)
        {
            if (user.Id == Context.User.Id)
            {
                await ReplyAsync("You can't jeff yourself :joy:");
                return;
            }
            else if (user.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync("You can't just jeff me like that.");
                return;
            }
            string imgUrl = "https://pbs.twimg.com/profile_images/880813322194612224/mgXPDJRq_400x400.jpg";
            string textStr = user.Mention + ", You just got jeffed by " + Context.User.Mention;
            Embed e = _lib.CreateEmbedWithImage("Jeff", textStr, imgUrl);

            await ReplyAsync("", embed: e);

        }

        [Command("acronym", RunMode = RunMode.Async)][Name("acronym")][Summary("A game of acronym!")]
        public async Task AcronymCmd ()
        {
            //Stage 1
            IUserMessage msg = await Context.Channel.SendMessageAsync("Welcome to the Acronym Game! In a few seconds i will give 6 letters for you to make an acronym (You can do this with multiple people!)");
            string acroLetters = _lib.GetRandomLetters(6);
            await Task.Delay(TimeSpan.FromSeconds(7.5));

            //Stage 2
            await msg.ModifyAsync(x => x.Content = $":timer: *You have 1 minute to make an acronym with the following!* **{acroLetters}** *(Only 10 submissions allowed)*");
            await Task.Delay(TimeSpan.FromSeconds(5));
            IUserMessage msg2 = await Context.Channel.SendMessageAsync("To submit an acronym, send your message starting with '*'. After that, just write your acronym.");
            await Task.Delay(TimeSpan.FromMinutes(1));

            //Stage 3
            await msg2.DeleteAsync();
            var messages = await Context.Channel.GetMessagesAsync(10).FlattenAsync();
            messages = messages.Where(x => x.Content.StartsWith("*"));
            //Get Message Count
            int messageCount = messages.Count(m => m.Content.StartsWith("*"));
            int winnerNum = new Random().Next(-1, messageCount + 1);
            int otherShit = 0;
            foreach (var message in messages)
            {
                if (message == null) continue;
                if (winnerNum == otherShit)
                {
                    if (message.Author.IsBot) continue;
                    await msg.ModifyAsync(x => x.Content = $"{message.Author.Mention} has won the the acronym with '{message.Content}'");
                    return;
                }
                otherShit++;
            }
        }

        [Command("stab")][Name("stab")][Summary("Stabs an user.")]
        public async Task StabCmd(SocketUser usr)
        {
            if (usr.Id == Context.User.Id)
            {
                await ReplyAsync("Yo bro that's fucked up.");
                return;
            }
            else if (usr.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync("I'm unstabbable.");
                return;
            }
            string text = usr.Username + ", you just got stabbed by " + Context.User.Username + "!";
            string imgUrl = "https://media.giphy.com/media/xUySTCy0JHxUxw4fao/giphy.gif";
            
            Embed e = _lib.CreateEmbedWithImage("Stab Command", text, imgUrl);
            await ReplyAsync("", embed: e);
        }
    }
}
