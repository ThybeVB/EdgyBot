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
                Console.WriteLine(resString);
                webClient.Dispose();

                return true;
            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "List API", e.Message);
                new LibEdgyBot().Log(msg);
            }

            return false;
        }
    }
}
