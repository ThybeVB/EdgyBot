using System.IO;
using Newtonsoft.Json.Linq;

namespace EdgyBot.Services
{
    public class LocalizationService
    {
        private readonly string path = "C:/EdgyBot/Localization/";

        public static JObject EnglishUS;
        public static JObject DutchBE;

        public LocalizationService ()
        {
            LoadAllLanguages();
        }

        public void LoadAllLanguages ()
        {
            EnglishUS = FromJson(File.ReadAllText(path + "en_US.json"));
            DutchBE = FromJson(File.ReadAllText(path + "nl_BE.json"));
        }

        public JObject FromJson (string rawJson)
        {
            JObject rss = JObject.Parse(rawJson);
            return rss;
        }
    }
}
