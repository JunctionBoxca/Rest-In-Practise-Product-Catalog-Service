using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Model
{
    [TestFixture]
    public class EventBufferTests
    {
        [Test]
        public void CanAddAndTakeEvent()
        {
            Event evnt = new EventBuilder().Build();
            
            EventBuffer buffer = new EventBufferBuilder().Build();
            buffer.Add(evnt);

            IEnumerable<Event> events = buffer.Take(1);

            Assert.AreEqual(1, events.Count());
            Assert.AreSame(evnt, events.First());
        }
        
        [Test]
        public void WhenNumberOfEventsInBufferExceedsBatchSizeShouldReturnBatchfulOfEvents()
        {
            EventBuffer buffer = new EventBufferBuilder().WithNumberOfEvents(QueryingEvents.BatchSize + 1).Build();

            IEnumerable<Event> events = buffer.Take(QueryingEvents.BatchSize);

            Assert.AreEqual(QueryingEvents.BatchSize, events.Count());           
        }

        [Test]
        public void WhenNumberOfEventsInBufferIsLessThanBatchSizeShouldReturnAllEventsInRepositoryOldestFirst()
        {
            Event oldestEvent = new EventBuilder().Build();
            Event newestEvent = new EventBuilder().Build();

            EventBuffer buffer = new EventBufferBuilder().WithEvents(oldestEvent, newestEvent).Build();

            IEnumerable<Event> events = buffer.Take(QueryingEvents.BatchSize);

            Assert.AreEqual(2, events.Count());
            Assert.AreSame(oldestEvent, events.ElementAt(0));
            Assert.AreSame(newestEvent, events.ElementAt(1));
        }

        [Test]
        public void IfThereAreNoEventsInBufferShouldReturnZeroEvents()
        {
            EventBuffer buffer = new EventBufferBuilder().Build();

            IEnumerable<Event> events = buffer.Take(QueryingEvents.BatchSize);

            Assert.AreEqual(0, events.Count());
        }
    }
}