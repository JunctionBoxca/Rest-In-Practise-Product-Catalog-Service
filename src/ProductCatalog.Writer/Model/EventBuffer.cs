using System.Collections.Generic;

namespace ProductCatalog.Writer.Model
{
    public class EventBuffer : IEventBuffer
    {
        private readonly Queue<Event> events;

        public EventBuffer()
        {
            events = new Queue<Event>();
        }

        public void Add(Event evnt)
        {
            events.Enqueue(evnt);
        }

        public IEnumerable<Event> Take(int batchSize)
        {
            int count = events.Count;
            int size = count < batchSize ? count : batchSize;

            Queue<Event> results = new Queue<Event>(size);

            for (int i = 0; i < size; i++)
            {
                results.Enqueue(events.Dequeue());
            }
            return results;
        }
    }
}