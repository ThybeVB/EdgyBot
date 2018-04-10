using Discord;
using System;
using System.Timers;

namespace EdgyBot.Handler
{
    public class EdgyPinger
    {
        public EdgyPinger ()
        {
            //new LibEdgyBot().EdgyLog(LogSeverity.Info, "Ping!");
            Timer timer = new Timer();
            timer.Interval = TimeSpan.FromMinutes(15).TotalMilliseconds;
            timer.Elapsed += PingClient;
            timer.Start();
        }

        private void PingClient(object sender, ElapsedEventArgs e)
        {
            new LibEdgyBot().EdgyLog(LogSeverity.Info, "Pinged Client");
        }
    }
}
