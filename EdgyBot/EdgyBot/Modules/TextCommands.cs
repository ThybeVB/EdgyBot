using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System;

namespace EdgyBot.Modules
{
    public class TextCommands : ModuleBase<SocketCommandContext>
    {
        //This command is not done, just some testing
        [Command("myinfo")]
        public async Task HelloCMD()
        {
            string username = Context.Message.Author.Username;
            string disc = Context.Message.Author.Discriminator;
            string joined = Context.Message.Author.CreatedAt.ToString();

            EmbedBuilder e = new EmbedBuilder();
            e.Author = new EmbedAuthorBuilder().WithIconUrl("https://images-ext-2.discordapp.net/external/Z8pafsoqXGAm3HUrpgedv02zhz9FxHQwKjNJxfn9CYE/https/cdn.discordapp.com/icons/238345584652713984/3458909894ab833b363ab21a6846b01d.jpg?width=80&height=80").WithName("X4Bot");
            e.Color = new Color(0x661967);
            e.AddField("Username", username);
            e.AddField("Discriminator", disc);
            e.AddField("Created At", joined);
            Embed a = e.Build();
            await ReplyAsync("", embed: a);

            Console.WriteLine(username + " used myinfo cmd");
        }
    }
}
