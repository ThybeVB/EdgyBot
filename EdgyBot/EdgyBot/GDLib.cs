using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EdgyBot
{
    //WIP
    public class GDLib
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        /// <summary>
        /// Gets the first Geometry Dash user based on their username.
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public async Task<string> GetGJUsers (string strInput)
        {
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

            return targetAccountID;
        }
        /// <summary>
        /// Gets an users info from the Geometry Dash Database.
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public async Task<string[]> getGJUserInfo(string inputStr)
        {
            var getUserValues = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"targetAccountID", inputStr},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getUserContent = new FormUrlEncodedContent(getUserValues);
            HttpResponseMessage getUserResponse = await _client.PostAsync("http://boomlings.com/database/getGJUserInfo20.php", getUserContent);
            string getUserResponseString = await getUserResponse.Content.ReadAsStringAsync();
            string[] finalResult = getUserResponseString.Split(':');
            return finalResult;
        }
        /// <summary>
        /// Gets Scores from the Geometry Dash Leaderboards
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<string[]> getGJScores20 (string type, int count)
        {
            var top10ReqDict = new Dictionary<string, string>
            {
                {"gameVersion", "21"},
                {"binaryVersion", "35"},
                {"gdw", "0"},
                {"accountID", _lib.GetGDAccID()},
                {"gjp", _lib.GetGJP()},
                {"type", type},
                {"count", count.ToString()},
                {"secret", "Wmfd2893gb7"}
            };
            FormUrlEncodedContent getScoresContent = new FormUrlEncodedContent(top10ReqDict);
            HttpResponseMessage getScoresResponse = await _client.PostAsync("http://boomlings.com/database/getGJScores20.php", getScoresContent);
            string getScoresResponseString = await getScoresResponse.Content.ReadAsStringAsync();
            string[] users = getScoresResponseString.Split('|');
            return users;
        }
    }
}
