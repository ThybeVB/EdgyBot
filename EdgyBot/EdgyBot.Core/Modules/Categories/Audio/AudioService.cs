using System;
using System.Collections.Generic;
using System.Text;
using Victoria;

namespace EdgyBot.Services
{
    public class AudioService
    {
        private Lavalink _lavalink;
        private LavaNode _node;

        public AudioService (Lavalink lavalink)
        {
            _lavalink = lavalink;
            _node = Core.Handler.EventHandler.Node;
        }

        public LavaNode GetNode ()
        {
            if (_node == null)
                _node = Core.Handler.EventHandler.Node;

            return _node;
        }
    }
}
