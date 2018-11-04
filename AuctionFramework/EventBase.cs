using System.Collections.Generic;

namespace AuctionFramework
{
    public abstract class EventBase
    {
        public EventBase()
        {
            Attributes = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Attributes { get; }
    }
}
