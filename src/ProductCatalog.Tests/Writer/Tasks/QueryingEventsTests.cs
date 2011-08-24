using System.Linq;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class QueryingEventsTests
    {
        [Test]
        public void IfBufferIsEmptyShouldTransitionToTerminate()
        {
            EventBuffer buffer = new EventBufferBuilder().Build();

            ITask start = new QueryingEvents();
            ITask end = start.Execute(new InMemoryFileSystem(), buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (Terminate), end);
        }

        [Test]
        public void IfBufferContainsEventsShouldTransitionToIdentifyingCurrentFeed()
        {
            int eventCount = 2;
            EventBuffer buffer = new EventBufferBuilder().WithNumberOfEvents(eventCount).Build();

            ITask start = new QueryingEvents();
            ITask end = start.Execute(new InMemoryFileSystem(), buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (IdentifyingRecentEventsFeed), end);
            Assert.AreEqual(eventCount, end.GetEvents().Count());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new QueryingEvents().IsLastTask);
        }
    }
}