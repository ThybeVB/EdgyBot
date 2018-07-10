using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Discord;

namespace EdgyCore.Handler.Pinger
{
    public class JsonHelper
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        private string urlInput = "";

        public JsonHelper (string url)
        {
            urlInput = url;
        }

        public void postListcord(Dictionary<string, object> dictData)
        {
            byte[] resByte;
            string resString;
            byte[] reqString;

            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", EdgyBot.Credentials.listcordToken);
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dictData, Formatting.Indented));
                resByte = webClient.UploadData(urlInput, "post", reqString);
                resString = Encoding.Default.GetString(resByte);
                webClient.Dispose();

                LogMessage log = new LogMessage(LogSeverity.Info, "Listcord API", "Success");
                _lib.Log(log);

            }
            catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, "BLSP API", e.Message);
                new LibEdgyBot().Log(msg);
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
