using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Discord;
using Newtonsoft.Json;

namespace EdgyCore.Handler.Pinger
{
    public class JsonHelper
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private string urlToPost = "";

        public JsonHelper (string url)
        {
            urlToPost = url;
        }

        public bool postDataBotsForDiscord (Dictionary<string, object> dictData)
        {
            WebClient webClient = new WebClient();
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credientals.bfdToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(this.urlToPost, "post", reqString);
                resString = Encoding.Default.GetString(resByte);
                webClient.Dispose();

                LogMessage log = new LogMessage(LogSeverity.Info, "BFD API", "Success");
                _lib.Log(log);

                return true;

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "BFD API", e.Message);
                new LibEdgyBot().Log(msg);
            }

            return false;
        }

        public bool postDataDiscordBots (Dictionary<string, object> dictData)
        {
            WebClient webClient = new WebClient();
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credientals.dbToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(this.urlToPost, "post", reqString);
                resString = Encoding.Default.GetString(resByte);

                LogMessage log = new LogMessage(LogSeverity.Info, "Discord Bots API", "Success");
                _lib.Log(log);

                webClient.Dispose();

                return true;
            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "Discord Bots API", e.Message);
                new LibEdgyBot().Log(msg);
            }

            return false;
        }
    }
}
