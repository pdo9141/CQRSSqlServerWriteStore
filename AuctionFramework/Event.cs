using System;

namespace AuctionFramework
{
    public class Event
    {
        public long SequenceID { get; set; }

        public Guid AggregateID { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public int Version { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
