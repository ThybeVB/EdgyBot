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
        LISTCORD,
        BFD
    }

    public class JsonHelper
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private string urlInput = "";

        public JsonHelper (string url)
        {
            urlInput = url;
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

        public string getRandomShibeUrl ()
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
                LogMessage msg = new LogMessage(LogSeverity.Error, "Shibe GET", e.Message);
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

        public void postDataSpaceList(Dictionary<string, object> dictData)
        {
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credentials.blsToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(urlInput, "post", reqString);
                resString = Encoding.Default.GetString(resByte);
                webClient.Dispose();

                LogMessage log = new LogMessage(LogSeverity.Info, "BLSP API", "Success");
                _lib.Log(log);

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "BLSP API", e.Message);
                new LibEdgyBot().Log(msg);
            }
        }

        public bool postDataBotsForDiscord (Dictionary<string, object> dictData)
        {
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credentials.bfdToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(urlInput, "post", reqString);
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
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credentials.dbToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(urlInput, "post", reqString);
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
