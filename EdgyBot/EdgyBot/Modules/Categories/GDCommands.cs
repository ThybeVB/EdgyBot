using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace EdgyBot.Modules.Categories
{
    public class GDCommands : ModuleBase<SocketCommandContext>
    {
        libEdgyBot lib = new libEdgyBot();
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
                {"accountID", "3117595"},
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
    }
}
