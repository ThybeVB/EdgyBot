using EdgyBot.Core;

namespace EdgyBot
{
    class Program
    {
        private static void Main()
            => new Bot().StartAsync().GetAwaiter().GetResult();
    }
}