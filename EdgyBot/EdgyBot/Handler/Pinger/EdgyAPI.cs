using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Discord;
using EdgyCore.Models;
using Newtonsoft.Json;

namespace EdgyCore.Handler
{
    public class EdgyAPI 
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        public async Task PostStatsAsync (StatsModel stats) 
        {
            byte[] resByte;
            string resString;
            byte[] reqString;
            
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers.Add("content-type", "application/json");
                webClient.Headers.Add("Authorization", Environment.GetEnvironmentVariable("EdgyBot_ApiToken", EnvironmentVariableTarget.User));
                reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(stats, Formatting.Indented));
                resByte = webClient.UploadData("http://localhost:3000/api/post_status", "post", reqString);
                resString = Encoding.Default.GetString(resByte);
                webClient.Dispose();

            } catch (Exception e)
            {
                LogMessage msg = new LogMessage(LogSeverity.Error, $"EDGYBOT API POST", e.Message);
                await new LibEdgyBot().Log(msg);
            }
        }
    }
}