using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using EdgyCore.Common;
using EdgyCore.Lib;

namespace EdgyCore.Lib
{
    public class GDLib
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly LibEdgyCore _coreLib = new LibEdgyCore();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        private readonly string gjSecret = "Wmfd2893gb7";

        public async Task<GDAccount[]> GetGJUsersAsync (string strInput) 
        {

            var gjUsersDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"str", strInput},
                {"total", "0"},
                {"page", "0"},
                {"secret", gjSecret}
            };

            FormUrlEncodedContent gjUsersContent = new FormUrlEncodedContent(gjUsersDict);
            HttpResponseMessage gjUsersResponse = await _client.PostAsync("http://boomlings.com/database/getGJUsers20.php", gjUsersContent);
            string responseString = await gjUsersResponse.Content.ReadAsStringAsync();
            string[] accountStrings = responseString.Split('|');

            int arrayLength = 0;
            GDAccount[] accounts = new GDAccount[10];
            foreach (string str in accountStrings) 
            {
                if (string.IsNullOrEmpty(str))
                    continue;
                
                string[] accountInfo = str.Split(':');

                GDAccount userInfo = await getGJUserInfoAsync(accountInfo[21]);

                GDAccount acc = new GDAccount
                {
                    accountID = accountInfo[21],
                    userID = accountInfo[3],
                    username = accountInfo[1],

                    stars = userInfo.stars,
                    diamonds = userInfo.diamonds,
                    userCoins = userInfo.userCoins,
                    coins = userInfo.coins,
                    demons = userInfo.demons,
                    creatorPoints = userInfo.creatorPoints,

                    youtubeUrl = userInfo.youtubeUrl,
                    twitterUrl = userInfo.twitterUrl,
                    twitchUrl = userInfo.twitchUrl
                };

                accounts[arrayLength] = acc;
                arrayLength++;
            }

            return accounts;
        }

        public async Task<GJComment> GetMostRecentComment (string accID)
        {
            var mostLikedCommentValues = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _coreLib.GetGDAccID()},
                {"page", "0"},
                {"total", "0"},
                {"secret", gjSecret}
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(mostLikedCommentValues);
            HttpResponseMessage response = await _client.PostAsync("http://boomlings.com/database/getGJAccountComments20.php", content);
            string responseString = await response.Content.ReadAsStringAsync();
            string[] mem = responseString.Split('|');
            string single = mem[0];
            string[] contents = single.Split('~');

            string decoded = _lib.DecodeB64(contents[2]);

            GJComment comment = new GJComment
            {
                comment = decoded,
                likes = contents[3]
            };

            return comment;
        }

        public async Task<GDAccount> getGJUserInfoAsync (string accID) 
        {
             var getUserValues = new Dictionary<string, string>
             {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _coreLib.GetGDAccID()},
                {"gjp", _coreLib.GetGJP()},
                {"targetAccountID", accID},
                {"secret", gjSecret}
            };
            FormUrlEncodedContent getUserContent = new FormUrlEncodedContent(getUserValues);
            HttpResponseMessage getUserResponse = await _client.PostAsync("http://boomlings.com/database/getGJUserInfo20.php", getUserContent);
            string getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();
            string[] finalResult = getUserResponseString.Split(':');

            GDAccount acc = new GDAccount();
            acc.youtubeUrl = finalResult[27];
            acc.twitterUrl = finalResult[53];
            acc.twitchUrl = finalResult[55];

            acc.stars = finalResult[13];
            acc.diamonds = finalResult[15];
            acc.userCoins = finalResult[7];
            acc.coins = finalResult[5];
            acc.demons = finalResult[17];
            acc.creatorPoints = finalResult[19];

            return acc;
        }

        public async Task<string[]> getGJScores20(string type, int count)
        {
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _coreLib.GetGDAccID()},
                {"gjp", _coreLib.GetGJP()},
                {"type", type},
                {"count", count.ToString()},
                {"secret", gjSecret}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
            return users;
        }

        public async Task<string[]> getGJComments21(string levelID)
        {
            var gjCommentsDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"page", "0"},
                {"total", "0"},
                {"secret", gjSecret},
                {"mode", "1"},
                {"levelID", levelID},
                {"count", "10"}
            };
            FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(gjCommentsDict);
            HttpResponseMessage responseMessage = await _client.PostAsync("http://boomlings.com/database/getGJComments21.php", encodedContent);
            string responseStr = await responseMessage.Content.ReadAsStringAsync();
            string[] messages = responseStr.Split('|');
            return messages;
        }

        public async Task<string[]> getGJLevels21(string strInput)
        {
            var gjLevelsDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"type", "0"},
                {"str", strInput},
                {"diff", "-"},
                {"len", "-"},
                {"page", "0"},
                {"total", "0"},
                {"unCompleted", "0"},
                {"onlyCompleted", "0"},
                {"featured", "0"},
                {"original", "0"},
                {"twoPlayer", "0"},
                {"coins", "0"},
                {"epic", "0"},
                {"demonFilter", "1"},
                {"secret", gjSecret}
            };
            FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(gjLevelsDict);
            HttpResponseMessage responseMessage = await _client.PostAsync("http://boomlings.com/database/getGJLevels21.php", encodedContent);
            string responseStr = await responseMessage.Content.ReadAsStringAsync();
            string[] levels = responseStr.Split('|');
            return levels;
        }

        public async Task<string> getGJLevel21(string strInput)
        {
            var gjLevelsDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"type", "0"},
                {"str", strInput},
                {"diff", "-"},
                {"len", "-"},
                {"page", "0"},
                {"total", "0"},
                {"unCompleted", "0"},
                {"onlyCompleted", "0"},
                {"featured", "0"},
                {"original", "0"},
                {"twoPlayer", "0"},
                {"coins", "0"},
                {"epic", "0"},
                {"demonFilter", "1"},
                {"secret", gjSecret}
            };
            FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(gjLevelsDict);
            HttpResponseMessage responseMessage = await _client.PostAsync("http://boomlings.com/database/getGJLevels21.php", encodedContent);
            string responseStr = await responseMessage.Content.ReadAsStringAsync();
            string[] levels = responseStr.Split('|');
            return levels[0];
        }
    }
}
