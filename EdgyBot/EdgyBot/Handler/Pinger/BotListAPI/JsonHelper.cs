using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Discord;

namespace EdgyCore.Handler.Pinger
{
    public enum Botlist
    {
        LISTSPACE,
        DISCORDBOTS,
        BFD,
        DBLCOM
    }

    public class JsonHelper
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private string urlInput = "";

        public JsonHelper (string url)
        {
            urlInput = url;
        }

        public string getMinecraftUser(string username)
        {
            byte[] resByte;
            string resString;

            try
            {
                WebClient client = new WebClient();
                resByte = client.DownloadData(urlInput);
                resString = Encoding.Default.GetString(resByte);
                client.Dispose();
                return resString;
            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "GET REQ", e.Message);
                new LibEdgyBot().Log(msg);
                return null;
            }
        }

        public void postBotlist(Dictionary<string, object> dictData, Botlist botlist)
        {
            byte[] resByte;
            string resString;
            byte[] reqString;
            
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", _lib.GetListToken(botlist));
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(urlInput, "post", reqString);
                resString = Encoding.Default.GetString(resByte);
                webClient.Dispose();

                LogMessage log = new LogMessage(LogSeverity.Info, $"{botlist.ToString()} API", "Success");
                _lib.Log(log);

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, $"{botlist.ToString()} POST", e.Message);
                new LibEdgyBot().Log(msg);
            }
        }

        public string getJSONFromUrl ()
        {
            byte[] resByte;
            string resString;

            try
            {
                WebClient client = new WebClient();
                resByte = client.DownloadData(urlInput);
                resString = Encoding.Default.GetString(resByte);
                client.Dispose();

                return resString;

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "GET URL", e.Message);
                new LibEdgyBot().Log(msg);
                return msg.ToString();
            }
        }

        public string getRandomMemeImgFlip ()
        {
            byte[] resByte;
            string resString;

            try
            {
                WebClient webClient = new WebClient();

                resByte = webClient.DownloadData(urlInput);
                resString = Encoding.Default.GetString(resByte);

                webClient.Dispose();

                return resString;
            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "Imgflip GET", e.Message);
                new LibEdgyBot().Log(msg);
                return msg.ToString();
            }
        }

        public string getRandomDogAPI()
        {
            byte[] resByte;
            string resString;

            try
            {
                WebClient webClient = new WebClient();

                resByte = webClient.DownloadData(urlInput);
                resString = Encoding.Default.GetString(resByte);

                webClient.Dispose();

                return resString;

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "DogCEO GET", e.Message);
                new LibEdgyBot().Log(msg);
                return msg.ToString();
            }
        }
    }
}
