using EdgyBot.Core;

namespace EdgyBot
{
    public class Program
    {
        private static void Main ()
            => new Bot().StartAsync().GetAwaiter().GetResult();
    }
}