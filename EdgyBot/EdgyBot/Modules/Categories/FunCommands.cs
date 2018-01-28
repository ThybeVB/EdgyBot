using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System;
using Discord.WebSocket;
using System.Text;

namespace EdgyBot.Modules.Categories
{
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

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
                if (letter == '?' || letter == '!' || letter == '.')
                {
                    await ReplyAsync("Message cannot contain such symbols.");
                    return;
                }
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
                await ReplyAsync("HAHAHAHAHAHAHAH NO.");
                return;
            }
            string imgUrl = "http://monstahhhbot.890m.com/MonstahhhFiles/jeff.jpg";
            string textStr = user.Mention + ", You just got jeffed by " + Context.User.Mention;
            Embed e = _lib.CreateEmbedWithImage("Jeff", textStr, imgUrl);

            await ReplyAsync("", embed: e);

        }
        [Command("lol")][Name("lol")][Summary("useless")]
        public async Task LolCmd(IGuildUser user)
        {
            if (user.Id == Context.Message.Author.Id)
            {
                await ReplyAsync("Nah m8 why would u lol urself");
            }
            else
            {
                await Context.Message.DeleteAsync();
                await ReplyAsync(user.Mention + " ....lol");
            }
        }
        [Command("kys")]
        [Name("kys")]
        [Summary("Tells EdgyBot to go kill himself.")]
        public async Task KysCmd()
        {
            await ReplyAsync("NO GO FUCK YOURSELF");
        }
        [Command("stab")][Name("stab")][Summary("Stabs an user.")]
        public async Task StabCmd(SocketUser usr = null)
        {
            if (usr == null)
            {
                await ReplyAsync("You need to mention an user!\nTry **e!stab @User123**.");
                return;
            }
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
            Embed e = _lib.CreateEmbedWithImage("Stab", text, imgUrl);

            await ReplyAsync("", embed: e);
        }
        [Command("gay")][Name("gay")][Summary("Tells somebody they're gay.")]
        public async Task GayCmd(IGuildUser usr)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(usr.Mention + ", ur gay :joy: :ok_hand:");
        }
    }
}
