using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class GDCommands : ModuleBase<SocketCommandContext>
    {
        LibEdgyBot lib = new LibEdgyBot();
        HttpClient client = new HttpClient();

        [Command("profile")]
        public async Task ProfileGDCMD (string strInput = null)
        {
            if (strInput == null)
            {
                await ReplyAsync("**ERROR:** Please enter an user.");
                return;
            }
            #region Collect Data
            #region GetGJUsers
            var gjUsersDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"str", strInput},
                {"total", "0"},
                {"page", "0"},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent gjUsersContent = new FormUrlEncodedContent(gjUsersDict);
            HttpResponseMessage gjUsersResponse = await client.PostAsync("http://boomlings.com/database/getGJUsers20.php", gjUsersContent);
            string responseString = await gjUsersResponse.Content.ReadAsStringAsync();
            string[] accountStuff = responseString.Split(':');
            string targetAccountID = accountStuff[21];
            #endregion
            #region getGJUserInfo
            var getUserValues = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", lib.getGDAccID()},
                {"gjp", lib.getGJP()},
                {"targetAccountID", targetAccountID},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getUserContent = new FormUrlEncodedContent(getUserValues);
            HttpResponseMessage getUserResponse = await client.PostAsync("http://boomlings.com/database/getGJUserInfo20.php", getUserContent);
            string getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();
            string[] finalResult = getUserResponseString.Split(':');
            #endregion
            #endregion
            #region Embed
            EmbedBuilder eb = lib.setupEmbedWithDefaults(false);
            string gdpicurl = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";
            eb.ThumbnailUrl = gdpicurl;
            eb.AddInlineField("Username", finalResult[1]);
            eb.AddInlineField("Stars", finalResult[13]);
            eb.AddInlineField("Diamonds", finalResult[15]);
            eb.AddInlineField("User Coins", finalResult[7]);
            eb.AddInlineField("Coins", finalResult[5]);
            eb.AddInlineField("Demons", finalResult[17]);
            eb.AddInlineField("Creator Points", finalResult[19]);
            #region SocialMediaChecks
            if (finalResult[27] != null && finalResult[27] != "") eb.AddInlineField("YouTube", "[YouTube](https://www.youtube.com/channel/" + finalResult[27] + ")"); else eb.AddInlineField("YouTube", "None");
            if (finalResult[55] != null && finalResult[55] != "") eb.AddInlineField("Twitch", $"[{finalResult[55]}](https://twitch.tv/" + finalResult[55] + ")"); else eb.AddInlineField("Twitch", "None");
            if (finalResult[53] != null && finalResult[53] != "") eb.AddInlineField("Twitter", $"[@{finalResult[53]}](https://www.twitter.com/@" + finalResult[53] + ")"); else eb.AddInlineField("Twitter", "None");
            #endregion 
            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.Text = $"User ID: {finalResult[3]}, Account ID: {targetAccountID}";
            footer.IconUrl = gdpicurl;
            eb.Footer = footer;
            #endregion
            Embed e = eb.Build();
            await ReplyAsync("", embed: e);
        }
        [Command("top10")]
        public async Task Top10GDCMD()
        {
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", lib.getGDAccID()},
                {"gjp", lib.getGJP()},
                {"type", "top"},
                {"count", "10"},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
            int lbPlace = 1;
            EmbedBuilder e = lib.setupEmbedWithDefaults();
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
            e.ThumbnailUrl = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("topplayers")]
        public async Task TopGDCMD(int count)
        {
            if (count > 25)
            {
                Embed aErr = lib.createEmbedWithText("Top " + count.ToString(), "The number you entered is too big.\n[MAX = 25]", false);
                await ReplyAsync("", embed: aErr);
                return;
            } else if (count <= 0)
            {
                Embed aErr1 = lib.createEmbedWithText("Top " + count.ToString(), "The number you entered is invalid.", false);
                await ReplyAsync("", embed: aErr1);
                return;
            }
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", lib.getGDAccID()},
                {"gjp", lib.getGJP()},
                {"type", "top"},
                {"count", count.ToString()},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
            int lbPlace = 1;
            EmbedBuilder e = lib.setupEmbedWithDefaults();
            foreach (var user in users)
            {
                if (user == "") continue;
                var userData = user.Split(':');
                var username = userData[1];
                var stars = userData[23];
                var placeWording = "nd";
                if (lbPlace == 3) placeWording = "d";
                if (lbPlace >= 4) placeWording = "th";
                if (lbPlace == 1) placeWording = "st";
                e.AddField(lbPlace + placeWording + " Place", $"**{username}**" + " with " + $"**{stars}" + " stars**");
                lbPlace++;
            }
            e.ThumbnailUrl = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("top10creators")]
        public async Task Top10CreatorsCMD()
        {
            #region Request
            var values = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", lib.getGDAccID()},
                {"gjp", lib.getGJP()},
                {"type", "creators"},
                {"count", "10"},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("http://boomlings.com/database/getGJScores20.php", content);
            string responseString = await response.Content.ReadAsStringAsync();
            #endregion
            string[] users = responseString.Split('|');
            int place = 1;
            EmbedBuilder e = lib.setupEmbedWithDefaults();
            foreach (string user in users)
            {
                if (user == "") continue;
                var userData = user.Split(':');
                var username = userData[1];
                var cp = userData[25];
                var placeWording = "nd";
                if (place == 3) placeWording = "rd";
                if (place >= 4) placeWording = "th";
                if (place == 1) placeWording = "st";
                e.AddField(place + placeWording + " Place", $"**{username}** with **{cp} Creator Points**");
                
                place++;
            }
            e.ThumbnailUrl = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
        [Command("topcreators")]
        public async Task TopCreatorsCMD(int count)
        {
            if (count > 25)
            {
                var aErr = lib.createEmbedWithText("Top " + count.ToString(), "The number you entered is too big.\n[MAX = 25]", false);
                await ReplyAsync("", embed: aErr);
                return;
            }
            else if (count <= 0)
            {
                var aErr1 = lib.createEmbedWithText("Top " + count.ToString(), "The number you entered is invalid.", false);
                await ReplyAsync("", embed: aErr1);
                return;
            }
            #region Request
            var values = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", lib.getGDAccID()},
                {"gjp", lib.getGJP()},
                {"type", "creators"},
                {"count", count.ToString()},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("http://boomlings.com/database/getGJScores20.php", content);
            string responseString = await response.Content.ReadAsStringAsync();
            #endregion
            string[] users = responseString.Split('|');
            int place = 1;
            EmbedBuilder e = lib.setupEmbedWithDefaults();
            foreach (string user in users)
            {
                if (user == "") continue;
                var userData = user.Split(':');
                var username = userData[1];
                var cp = userData[25];
                var placeWording = "nd";
                if (place == 3) placeWording = "rd";
                if (place >= 4) placeWording = "th";
                if (place == 1) placeWording = "st";
                e.AddField(place + placeWording + " Place", $"**{username}** with **{cp} Creator Points**");

                place++;
            }
            e.ThumbnailUrl = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";
            Embed a = e.Build();
            await ReplyAsync("", embed: a);
        }
    }
}