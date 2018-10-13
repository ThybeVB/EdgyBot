using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using EdgyBot.Core.Lib;

namespace EdgyBot.Modules
{
    [Name("Random Commands"), Summary("Commands that have different output every time")]
    public class RandomCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        
    }
}
