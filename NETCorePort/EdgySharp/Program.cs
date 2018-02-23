using System;
using Discord;
using System.Threading.Tasks;

namespace EdgySharp
{
    public class EdgyBot
    {
        private static void Main() => new EdgyBot().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            await Task.Delay(-1);
        }
    }  
}
