using System.Collections.Generic;

namespace ProductCatalog.Writer.Model
{
    public interface IEventBuffer
    {
        void Add(Event evnt);
        IEnumerable<Event> Take(int batchSize);
    }
}