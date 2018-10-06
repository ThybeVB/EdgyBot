using System;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using EdgyBot.Core.Lib;

namespace EdgyBot.Modules
{
    [Name("Fun Commands"), Summary("Commands used for FUNUNUNUN")]
    public class FunCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private string[] ballReplies = {"It is certain.", "It is decidedly so.", "Without a doubt.", "Yes - definitely.", 
            "You may rely on it.", "As I see it, yes.", "Most likely.", "Outlook good.", "Yes.", "Signs point to yes.", 
            "Reply hazy, try again", "Ask again later.", "Cannot predict now.", "Concentrate and ask again.", 
            "Don't count on it.", "My reply is no.", "My sources say no", "Outlook not so good.", "Very doubtful"};

        [Command("8ball")]
        [Name("8ball"), Summary("Ask a question to the 8ball")]
        public async Task EightBallCmd ([Remainder]string question) 
        {
            Random random = new Random();
            int index = random.Next(ballReplies.Count());

            string responseString = $"{Context.User.Mention}: {question}\n:8ball:: {ballReplies[index]}";
            await ReplyAsync(responseString);
        }

        [Command("duel", RunMode = RunMode.Async)]
        [Name("duel"), Summary("Challenges a user to a duel!")]
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

        [Command("clap")]
        [Name("clap"), Summary("Converts your sentence into claps")]
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

        [Command("vertical")]
        [Name("vertical"), Summary("Converts your message to a vertical one")]
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

        [Command("bigletter"), Alias("big", "emojify")]
        [Name("bigletter"), Summary("Converts your message to Emoji's")]
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

        [Command("juststop"), Alias("stopit", "timetostop")]
        [Name("juststop"), Summary("Tells somebody to **STOP**")]
        public async Task StopCmd(IGuildUser usr)
        {
            string stopUrl = "https://i.imgur.com/1TdHj1y.gif";
            Embed a = _lib.CreateEmbedWithImage(usr.Mention, stopUrl);
            await ReplyAsync("", embed: a);
        }

        [Command("jeff")]
        [Name("jeff"), Summary("Jeff's somebody. Yeah.")]
        public async Task JeffCmd(IGuildUser user)
        {
            if (user.Id == Context.User.Id)
            {
                await ReplyAsync("You can't jeff yourself :joy:");
                return;
            } else if (user.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync("You can't just jeff me like that.");
                return;
            }
            string imgUrl = "https://pbs.twimg.com/profile_images/880813322194612224/mgXPDJRq_400x400.jpg";
            string textStr = user.Mention + ", You just got jeffed by " + Context.User.Mention;
            Embed e = _lib.CreateEmbedWithImage("Jeff", textStr, imgUrl);

            await ReplyAsync("", embed: e);

        }

        [Command("acronym", RunMode = RunMode.Async)]
        [Name("acronym"), Summary("A game of acronym!\nThis command has known to be buggy in the past, so if you find a bug in it please use e!bugreport.")]
        public async Task AcronymCmd ()
        {
            #region Preparation
            //Stage 1
            IUserMessage msg = await Context.Channel.SendMessageAsync("Welcome to the Acronym Game! In 10 seconds i will give 6 letters for you to make an acronym (You can do this with multiple people!)");
            string acroLetters = _lib.GetRandomLetters(6);
            await Task.Delay(TimeSpan.FromSeconds(10));

            //Stage 2
            await msg.ModifyAsync(x => x.Content = $":timer: *You have 1 minute to make an acronym with the following letters!* **{acroLetters}** *(Only 10 submissions will be included)*");
            await Task.Delay(TimeSpan.FromSeconds(5));
            IUserMessage msg2 = await Context.Channel.SendMessageAsync("To submit an acronym, send your message starting with '*'. After that, just write your acronym.\nhttps://cdn.discordapp.com/attachments/412350471584481291/473955216518021130/unknown.png");
            await Task.Delay(TimeSpan.FromMinutes(1));
            await msg2.DeleteAsync();
            #endregion

            //Stage 3
            var messages = await Context.Channel.GetMessagesAsync(10).FlattenAsync();
            IMessage[] messagesObj = messages.Where(x => x.Content.StartsWith("*")).ToArray();

            //Get Message Count
            int messageCount = messagesObj.Count();
            int winnerNum = new Random().Next(-1, messageCount + 1);
            int otherShit = 0;
            foreach (var message in messagesObj)
            {
                if (message == null) continue;
                if (winnerNum == otherShit)
                {
                    if (message.Author.IsBot) continue;
                    await msg.ModifyAsync(x => x.Content = $"{message.Author.Mention} has won the the acronym with '{message.Content}'");
                    await ReplyAsync("Game End! Before you start another game, please delete the previous submissions, as they would be included in the next game.");
                    return;
                }
                otherShit++;
            }
        }

        [Command("bill")]
        [Name("bill"), Summary("Be like bill. (your name)")]
        public async Task BillCmd ([Remainder]string name = "Bill") 
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.ImageUrl = $"http://belikebill.azurewebsites.net/billgen-API.php?default=1&name={urlLinkBill(name)}";

            await ReplyAsync("", embed: eb.Build());
        }

        private string urlLinkBill (string name) 
        {
            char[] nameChars = name.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in nameChars) {
                if (c == ' ') {
                    sb.Append("%20");
                    continue;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        [Command("stab")]
        [Name("stab"), Summary("Lets you stab a user")]
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
