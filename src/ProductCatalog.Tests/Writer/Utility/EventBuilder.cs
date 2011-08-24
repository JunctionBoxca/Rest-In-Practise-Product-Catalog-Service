using System;
using ProductCatalog.Writer.Model;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class EventBuilder
    {
        private int id = 1;
        private string subject = "an event occurred";
        private DateTime timestamp = DateTime.Now;
        private EventBody body = new EventBodyBuilder().Build();
        
        public EventBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public EventBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public Event Build()
        {
            return new Event(id, subject, timestamp, body);
        }
    }
}