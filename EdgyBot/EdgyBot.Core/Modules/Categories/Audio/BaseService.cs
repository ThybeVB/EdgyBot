using System;
using HyperEx;
using Victoria;

namespace EdgyBot.Services
{
    [Inject]
    public class BaseService
    {
        internal virtual IServiceProvider Provider { get; set; }
        internal virtual Lavalink Lavalink { get; set; }
        internal virtual AudioService Audio { get; set; }
    }
}
