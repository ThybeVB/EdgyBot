using System.IO;
using Newtonsoft.Json.Linq;
using EdgyBot.Core.Lib;
using WinSCP;
using Discord;
using System;

namespace EdgyBot.Services
{
    public class LocalizationService
    {
        private readonly string path = "C:/EdgyBot/Localization/";
        private LibEdgyBot _lib = new LibEdgyBot();

        public static JObject EnglishUS;
        public static JObject DutchBE;

        public LocalizationService ()
        {
            DownloadLanguages();
            LoadAllLanguages();
        }

        private void DownloadLanguages ()
        {
            _lib.EdgyLog(LogSeverity.Info, "Connecting to EdgyBot FTP Server...");
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "ftp.example.com",
                UserName = "this so far",
                Password = "doesn't work",
            };
            _lib.EdgyLog(LogSeverity.Info, "Connected to EdgyBot FTP Server...");

            using (Session session = new Session())
            {
                _lib.EdgyLog(LogSeverity.Info, "Download languages from EdgyBot FTP Server...");
                session.Open(sessionOptions);
                try
                {
                    session.GetFiles("/edgybot/lang/*", @"C:\EdgyBot\Localization\*").Check();
                } catch (Exception e)
                {
                    _lib.EdgyLog(LogSeverity.Critical, "Failed to retrieve languages from EdgyBot FTP Server:" + e.Message);
                }
            }
        }

        private void LoadAllLanguages ()
        {
            EnglishUS = FromJson(File.ReadAllText(path + "en_US.json"));
            //DutchBE = FromJson(File.ReadAllText(path + "nl_BE.json"));
        }

        public JObject FromJson (string rawJson)
        {
            JObject rss = JObject.Parse(rawJson);
            return rss;
        }
    }
}
