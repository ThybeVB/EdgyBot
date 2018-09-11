// ReSharper disable StyleCop.SA1600
// ReSharper disable StyleCop.SA1516
namespace Discord.Addons.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Discord.WebSocket;

    public class ReactionCallbackData
    {
        private readonly ICollection<ReactionCallbackItem> items;

        public ReactionCallbackData(string text, Embed embed = null, bool expiresAfterUse = true, bool singleUsePerUser = true, TimeSpan? timeout = null, Func<SocketCommandContext, Task> timeoutCallback = null)
        {
            if (text == null && embed == null)
            {
                throw new Exception("Inline reaction must have message data");
            }

            SingleUsePerUser = singleUsePerUser;
            ExpiresAfterUse = expiresAfterUse;
            ReactorIDs = new List<ulong>();
            Text = text ?? "";
            Embed = embed;
            Timeout = timeout;
            TimeoutCallback = timeoutCallback;
            items = new List<ReactionCallbackItem>();
        }

        public IEnumerable<ReactionCallbackItem> Callbacks => items;
        public bool ExpiresAfterUse { get; }
        public bool SingleUsePerUser { get; }
        public List<ulong> ReactorIDs { get; }
        public string Text { get; }
        public Embed Embed { get; }
        public TimeSpan? Timeout { get; }
        public Func<SocketCommandContext, Task> TimeoutCallback { get; }

        public ReactionCallbackData WithCallback(IEmote reaction, Func<SocketCommandContext, SocketReaction, Task> callback)
        {
            var item = new ReactionCallbackItem(reaction, callback);
            items.Add(item);
            return this;
        }
    }
}