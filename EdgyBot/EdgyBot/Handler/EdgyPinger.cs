using Discord;
using System;
using System.Net.NetworkInformation;
using System.Timers;

namespace EdgyCore.Handler
{
    public class EdgyPinger
    {
        public EdgyPinger ()
        {
            //new LibEdgyBot().EdgyLog(LogSeverity.Info, "Ping!");
            Timer timer = new Timer();
            timer.Interval = TimeSpan.FromMinutes(25).TotalMilliseconds;
            timer.Elapsed += PingClient;
            timer.Start();
        }

        private void PingClient(object sender, ElapsedEventArgs e)
        {
            Ping p = new Ping();
            PingReply pR = p.Send("https://edgybotdocker.herokuapp.com");
            new LibEdgyBot().EdgyLog(LogSeverity.Info, pR.Buffer.ToString());
        }
    }
}
