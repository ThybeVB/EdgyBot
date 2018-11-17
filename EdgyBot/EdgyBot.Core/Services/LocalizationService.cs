using HyperEx;
using Newtonsoft.Json.Linq;

namespace EdgyBot.Services
{
    public class LocalizationService
    {
        public LocalizationService ()
        {
            LoadAllLanguages();
        }

        public void LoadAllLanguages ()
        {

        }

        public JObject FromJson (string rawJson)
        {
            JObject rss = JObject.Parse(rawJson);
            return rss;
        }
    }
}
