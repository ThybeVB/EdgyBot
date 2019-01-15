using System.IO;
using Newtonsoft.Json.Linq;
using EdgyBot.Core.Lib;
using WinSCP;
using Discord;
using System;
using FluentFTP;
using System.Net;

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
            //DownloadLanguages();
            LoadAllLanguages();
        }

        private void DownloadLanguages ()
        {
            //FtpClient client = new FtpClient("35.176.231.118");
            //client.Credentials = new NetworkCredential("monstah", "cuck");
            //client.Connect();
            //
            //client.DownloadFile(@"C:\EdgyBot\Localization\en_US.json", "/lang/en_US.json");

            //_lib.EdgyLog(LogSeverity.Info, "Connecting to EdgyBot FTP Server...");
            //SessionOptions sessionOptions = new SessionOptions
            //{
            //    Protocol = Protocol.Ftp,
            //    HostName = "35.176.231.118",
            //    UserName = "monstah",
            //    Password = "cuck",
            //    FtpMode = FtpMode.Passive
            //};
            //
            //using (Session session = new Session())
            //{
            //    try
            //    {
            //        session.Open(sessionOptions);
            //        _lib.EdgyLog(LogSeverity.Info, "Connected to EdgyBot FTP Server");
            //        _lib.EdgyLog(LogSeverity.Info, "Downloading languages from EdgyBot FTP Server...");
            //        session.GetFiles("/lang/*", @"C:\EdgyBot\Localization\*")/*.Check()*/;
            //    } catch (Exception e)
            //    {
            //        _lib.EdgyLog(LogSeverity.Critical, "Failed to retrieve languages from EdgyBot FTP Server: " + e.Message);
            //    }
            //}
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
