using System.Linq;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class RequeryingEventsTests
    {
        [Test]
        public void WhenBufferIsEmptyShouldTransitionToSavingCurrentFeed()
        {
            RecentEventsFeed feed = new RecentEventsFeedBuilder().WithNumberOfEntries(3).Build();

            ITask start = new RequeryingEvents(feed);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (SavingRecentEventsFeed), end);
            Assert.AreEqual(feed, end.GetRecentEventsFeed());
        }

        [Test]
        public void WhenBufferReturnsSomeEventsShouldTransitionToUpdatingCurrentFeed()
        {
            int eventCount = 2;
            EventBuffer buffer = new EventBufferBuilder().WithNumberOfEvents(eventCount).Build();
            
            RecentEventsFeed feed = new RecentEventsFeedBuilder().WithNumberOfEntries(3).Build();

            ITask start = new RequeryingEvents(feed);           
            ITask end = start.Execute(new InMemoryFileSystem(), buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof(UpdatingRecentEventsFeed), end);
            Assert.AreEqual(feed, end.GetRecentEventsFeed());
            Assert.AreEqual(eventCount, end.GetEvents().Count());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new RequeryingEvents(RecentEventsFeedBuilder.FullCurrentFeed()).IsLastTask);
        }
    }
}