using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core.Lib;
using EdgyBot.Core;
using EdgyBot.Core.Models;

namespace EdgyBot.Modules
{
    [Name("Geometry Dash Commands"), Summary("Commands related to Geometry Dash")]
    public class GDCommands : ModuleBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly GDLib _gdLib = new GDLib();

        private string gdThumbPic = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";

        [Command("profile", RunMode = RunMode.Async)]
        [Name("profile")] [Summary("Searches Geometry Dash's users for the user you entered.")]
        public async Task GDProfileCmd ([Remainder]string strInput = null)
        {
            GDAccount[] accounts = await _gdLib.GetGJUsersAsync(strInput);
            if (accounts == null) {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError("Geometry Dash Commands Error", ":x: Could not find a user with this username."));
                return;
            }
            GDAccount account = accounts[0];
            
            EmbedBuilder builder = _lib.SetupEmbedWithDefaults();
            string gdLogoUrl = gdThumbPic;
            builder.ThumbnailUrl = gdLogoUrl;
            
            builder.AddField("Username", account.username, true);
            builder.AddField("Stars", account.stars, true);
            builder.AddField("Diamonds", account.diamonds, true);
            builder.AddField("User Coins", account.userCoins, true);
            builder.AddField("Coins", account.coins, true);
            builder.AddField("Demons", account.demons, true);
            builder.AddField("Creator Points", account.creatorPoints, true);
            
            if (!string.IsNullOrEmpty(account.youtubeUrl)) builder.AddField("YouTube", "[YouTube](https://www.youtube.com/channel/" + account.youtubeUrl + ")", true); else builder.AddField("YouTube", "None", true);
            if (!string.IsNullOrEmpty(account.twitchUrl)) builder.AddField("Twitch", $"[{account.twitchUrl}](https://twitch.tv/" + account.twitchUrl + ")", true); else builder.AddField("Twitch", "None", true);
            if (!string.IsNullOrEmpty(account.twitterUrl)) builder.AddField("Twitter", $"[@{account.twitterUrl}](https://www.twitter.com/@" + account.twitterUrl + ")", true); else builder.AddField("Twitter", "None", true);

            GDComment comment = await _gdLib.GetMostRecentComment(account.accountID);
            if (comment != null)
                builder.AddField("Most Recent Account Comment", comment.comment + " | " + comment.likes + " likes");
            
            EmbedFooterBuilder embedFooterBuilder = new EmbedFooterBuilder
            {
                Text = $"User ID: {account.userID}, Account ID: {account.accountID}",
                IconUrl = gdLogoUrl
            };
            builder.Footer = embedFooterBuilder;
            
            await ReplyAsync("", embed: builder.Build());
        }

        [Command("top10players")]
        [Name("top10players")][Summary("Gives the current Top 10 Players in Geometry Dash")]
        public async Task GDTop10Cmd()
        {
            string[] users = await _gdLib.getGJScores20("top", 10);
            
            int lbPlace = 1;
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            foreach (string user in users)
            {
                if (user == "") continue;
                string[] userData = user.Split(':');
                string username = userData[1];
                string stars = userData[23];
                string placeWording = "nd";
                if (lbPlace == 3) placeWording = "d";
                if (lbPlace >= 4) placeWording = "th";
                if (lbPlace == 1) placeWording = "st";
                e.AddField(lbPlace + placeWording + " Place", $"**{username}**" + " with " + $"**{stars}" + " stars**");
                lbPlace++;
            }
            e.ThumbnailUrl = gdThumbPic;

            await ReplyAsync("", embed: e.Build());
        }

        [Command("topplayers")]
        [Name("topplayers")][Summary("Same thing as top10players, but it is based on the number you enter")]
        public async Task GDTopPlayersCmd(int count)
        {
            string countStr = count.ToString();
            if (count > 25)
            {
                Embed aErr = _lib.CreateEmbedWithText("Top " + countStr, "The number you entered is too big.\n[MAX = 25]");
                await ReplyAsync("", embed: aErr);
                return;
            } else if (count <= 0)
            {
                Embed aErr1 = _lib.CreateEmbedWithText("Top " + countStr, "The number you entered is invalid.");
                await ReplyAsync("", embed: aErr1);
                return;
            }
            string[] users = await _gdLib.getGJScores20("top", count);
            int lbPlace = 1;
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            foreach (var user in users)
            {
                if (user == "") continue;
                string[] userData = user.Split(':');
                string username = userData[1];
                string stars = userData[23];
                string placeWording = "nd";
                if (lbPlace == 3) placeWording = "d";
                if (lbPlace >= 4) placeWording = "th";
                if (lbPlace == 1) placeWording = "st";
                e.AddField(lbPlace + placeWording + " Place", $"**{username}**" + " with " + $"**{stars}" + " stars**");
                lbPlace++;
            }
            e.ThumbnailUrl = gdThumbPic;
            await ReplyAsync("", embed: e.Build());
        }

        [Command("top10creators")]
        [Name("top10creators")][Summary("Gives the current Top 10 Creators in Geometry Dash based on their Creator Points")]
        public async Task GDTop10CreatorsCmd()
        {
            string[] users = await _gdLib.getGJScores20("creators", 10);
            int place = 1;
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            foreach (string user in users)
            {
                if (user == "") continue;
                string[] userData = user.Split(':');
                string username = userData[1];
                string cp = userData[25];
                string placeWording = "nd";
                if (place == 3) placeWording = "rd";
                if (place >= 4) placeWording = "th";
                if (place == 1) placeWording = "st";
                e.AddField(place + placeWording + " Place", $"**{username}** with **{cp} Creator Points**");
                
                place++;
            }
            e.ThumbnailUrl = gdThumbPic;
            await ReplyAsync("", embed: e.Build());
        }

        [Command("topcreators")]
        [Name("topcreators")][Summary("Same thing as top10creators, but it is based on the number you entered")]
        public async Task GDTopCreatorsCmd(int count)
        {
            if (count > 25)
            {
                var aErr = _lib.CreateEmbedWithText("Top " + count, "The number you entered is too big.\n[MAX = 25]");
                await ReplyAsync("", embed: aErr);
                return;
            } else if (count <= 0)
            {
                var aErr1 = _lib.CreateEmbedWithText("Top " + count, "The number you entered is invalid.");
                await ReplyAsync("", embed: aErr1);
                return;
            }
            string[] users = await _gdLib.getGJScores20("creators", count);
            int place = 1;
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            foreach (string user in users)
            {
                if (user == "") continue;
                string[] userData = user.Split(':');
                string username = userData[1];
                string cp = userData[25];
                string wording = "nd";
                if (place == 3) wording = "rd";
                if (place >= 4) wording = "th";
                if (place == 1) wording = "st";
                e.AddField(place + wording + " Place", $"**{username}** with **{cp} Creator Points**");

                place++;
            }
            e.ThumbnailUrl = gdThumbPic;
            await ReplyAsync("", embed: e.Build());
        }

        [Command("topcomments", RunMode = RunMode.Async)]
        [Name("topcomments")][Summary("Shows the most liked comments on your Geometry Dash Level")]
        public async Task GDTopLevelCommentsCmd ([Remainder]string str = null)
        {
            if (str == null)
            {
                await ReplyAsync("Please enter the **name** or **ID** of a level!");
                return;
            }
            string level = await _gdLib.getGJLevel21(str);
            string[] levelInfo = level.Split(':');
            string levelID = levelInfo[1];

            string[] comments = await _gdLib.getGJComments21(levelID);
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.ThumbnailUrl = gdThumbPic;
            eb.Footer = new EmbedFooterBuilder ()
            {
                Text = "If you see usernames as numbers, it is probarbly a bug with RobTop's server. This problem usually occurs when the user is a Moderator."
            };
            foreach (string comment in comments)
            {
                if (comment == null) continue;
                string[] commentInfo = comment.Split('~');
                string username;
                username = commentInfo[14];
                try { eb.AddField(username, _lib.DecodeB64(commentInfo[1]) + " | Likes: " + commentInfo[5]); } catch
                {
                    eb.AddField(username, "**Error**: Invalid Comment.");
                }
            }
            await ReplyAsync("", embed: eb.Build());
        }
    }
}