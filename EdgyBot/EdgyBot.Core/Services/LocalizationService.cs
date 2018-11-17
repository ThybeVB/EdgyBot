using HyperEx;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EdgyBot.Services
{
    public class LocalizationService
    {
        private readonly string path = "C:/EdgyBot/Localization/";

        public JObject English;

        public LocalizationService ()
        {
            LoadAllLanguages();
        }

        public void LoadAllLanguages ()
        {
            English = FromJson(File.ReadAllText(path + "en_US.json"));
        }

        public JObject FromJson (string rawJson)
        {
            JObject rss = JObject.Parse(rawJson);
            return rss;
        }
    }
}
