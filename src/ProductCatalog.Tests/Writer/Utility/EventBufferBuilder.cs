using System.Collections.Generic;
using ProductCatalog.Writer.Model;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class EventBufferBuilder
    {
        private int eventCount;
        private IEnumerable<Event> events = new Event[] {};

        public EventBufferBuilder WithNumberOfEvents(int eventCount)
        {
            this.eventCount = eventCount;
            events = new Event[] {};
            return this;
        }

        public EventBufferBuilder WithEvents(params Event[] events)
        {
            this.events = events;
            eventCount = 0;
            return this;
        }

        public EventBuffer Build()
        {
            EventBuilder eventBuilder = new EventBuilder();

            EventBuffer buffer = new EventBuffer();
            Repeat.Times(eventCount).Action(() => buffer.Add(eventBuilder.Build()));

            foreach (var evnt in events)
            {
                buffer.Add(evnt);
            }

            return buffer;
        }
    }
}