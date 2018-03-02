using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class GDCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly HttpClient _client = new HttpClient();

        private string gdThumbPic = "https://lh5.ggpht.com/gSJ1oQ4a5pxvNHEktd21Gh36QbtZMMx5vqFZfe47VDs1fzCEeMCyThqOfg3DsTisYCo=w300";

        [Command("profile")][Name("profile")][Summary("Gives you a profile from Geometry Dash")]
        public async Task ProfileGDCMD (string strInput = null)
        {
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
            HttpResponseMessage gjUsersResponse = await _client.PostAsync("http://boomlings.com/database/getGJUsers20.php", gjUsersContent);
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
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"targetAccountID", targetAccountID},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getUserContent = new FormUrlEncodedContent(getUserValues);
            HttpResponseMessage getUserResponse = await _client.PostAsync("http://boomlings.com/database/getGJUserInfo20.php", getUserContent);
            string getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();
            string[] finalResult = getUserResponseString.Split(':');
            #endregion
            #endregion
            #region Embed
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
            footer.Text = $"User ID: {finalResult[3]}, Account ID: {targetAccountID}";
            footer.IconUrl = gdpicurl;
            eb.Footer = footer;
            #endregion
            Embed e = eb.Build();
            await ReplyAsync("", embed: e);
        }
        [Command("top10players")][Name("top10players")][Summary("Gives the current Top 10 players in Geometry Dash")]
        public async Task Top10GDCMD()
        {
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"type", "top"},
                {"count", "10"},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
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
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"type", "top"},
                {"count", count.ToString()},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
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
            #region Request
            var values = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"type", "creators"},
                {"count", "10"},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", content);
            string responseString = await response.Content.ReadAsStringAsync();
            #endregion
            string[] users = responseString.Split('|');
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
            #region Request
            var values = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"type", "creators"},
                {"count", count.ToString()},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", content);
            string responseString = await response.Content.ReadAsStringAsync();
            #endregion
            string[] usersStr = responseString.Split('|');
            int place = 1;
            EmbedBuilder e = _lib.SetupEmbedWithDefaults();
            foreach (string user in usersStr)
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
    }
}