using System.Collections.Generic;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class CreatingNewRecentEventsFeedTests
    {
        [Test]
        public void ShouldTransitionToUpdatingRecentEventsFeed()
        {
            IEnumerable<Event> events = new[] {new EventBuilder().Build()};

            ITask start = new CreatingNewRecentEventsFeed(events);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (UpdatingRecentEventsFeed), end);
            Assert.AreEqual(Id.InitialValue, end.GetRecentEventsFeed().GetFeedMapping().Id);
            Assert.AreEqual(events, end.GetEvents());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new CreatingNewRecentEventsFeed(new Event[] {}).IsLastTask);
        }
    }
}