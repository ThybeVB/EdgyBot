﻿using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class GDCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly GDLib _gdLib = new GDLib();

        private string gdThumbPic = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";

        [Command("profile")][Name("profile")][Summary("Gives you a profile from Geometry Dash")]
        public async Task ProfileGDCMD ([Remainder]string strInput = null)
        {
            string accID = await _gdLib.GetGJUsers(strInput);
            string[] finalResult = await _gdLib.getGJUserInfo(accID);

            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(false);
            string gdpicurl = gdThumbPic;
            eb.ThumbnailUrl = gdpicurl;

            eb.AddField("Username", finalResult[1], true);
            eb.AddField("Stars", finalResult[13], true);
            eb.AddField("Diamonds", finalResult[15], true);
            eb.AddField("User Coins", finalResult[7], true);
            eb.AddField("Coins", finalResult[5], true);
            eb.AddField("Demons", finalResult[17], true);
            eb.AddField("Creator Points", finalResult[19], true);

            if (!string.IsNullOrEmpty(finalResult[27])) eb.AddField("YouTube", "[YouTube](https://www.youtube.com/channel/" + finalResult[27] + ")", true); else eb.AddField("YouTube", "None", true);
            if (!string.IsNullOrEmpty(finalResult[55])) eb.AddField("Twitch", $"[{finalResult[55]}](https://twitch.tv/" + finalResult[55] + ")", true); else eb.AddField("Twitch", "None", true);
            if (!string.IsNullOrEmpty(finalResult[53])) eb.AddField("Twitter", $"[@{finalResult[53]}](https://www.twitter.com/@" + finalResult[53] + ")", true); else eb.AddField("Twitter", "None", true);
            EmbedFooterBuilder footer = new EmbedFooterBuilder();

            footer.Text = $"User ID: {finalResult[3]}, Account ID: {accID}";
            footer.IconUrl = gdpicurl;
            eb.Footer = footer;
            Embed e = eb.Build();

            await ReplyAsync("", embed: e);
        }
        [Command("top10players")][Name("top10players")][Summary("Gives the current Top 10 players in Geometry Dash")]
        public async Task Top10GDCMD()
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
            Embed a = e.Build();

            await ReplyAsync("", embed: a);
        }
        [Command("topplayers")][Name("topplayers")][Summary("Same thing as top10players, but it is based on your number")]
        public async Task TopGDCMD(int count)
        {
            string countStr = count.ToString();
            if (count > 25)
            {
                Embed aErr = _lib.CreateEmbedWithText("Top " + countStr, "The number you entered is too big.\n[MAX = 25]", false);
                await ReplyAsync("", embed: aErr);
                return;
            } else if (count <= 0)
            {
                Embed aErr1 = _lib.CreateEmbedWithText("Top " + countStr, "The number you entered is invalid.", false);
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
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("top10creators")][Name("top10creators")][Summary("Gives the current Top 10 Creators in Geometry Dash")]
        public async Task Top10CreatorsCMD()
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
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("topcreators")][Name("topcreators")][Summary("Same thing as top10creators, but it is based on your number.")]
        public async Task TopCreatorsCMD(int count)
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
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("topcomments")][Name("topcomments")][Summary("Shows the most liked comments on a Geometry Dash Level")]
        public async Task LevelCommentsCMD ([Remainder]string str = null)
        {
            if (str == null)
            {
                await ReplyAsync("Please enter the **name** or **ID** of a level!");
                return;
            }
            string[] levels = await _gdLib.getGJLevels21(str);
            string level = levels[0];
            string[] levelInfo = level.Split(':');
            string levelID = levelInfo[1];

            string[] comments = await _gdLib.getGJComments21(levelID);
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            int bug = 0;
            foreach (string comment in comments)
            {
                if (comment == null) continue;
                string[] commentInfo = comment.Split('~');
                string username;
                if (bug == 0)
                {
                    username = commentInfo[18];
                } else
                {
                    username = commentInfo[14];
                }          
                string encodedComment = commentInfo[1];
                string likeAmount = commentInfo[5];
                eb.AddField(username, encodedComment + " | Likes: " + likeAmount);

                bug++;
            }
            Embed e = eb.Build();
            await ReplyAsync("", embed: e);
        }
    }
}