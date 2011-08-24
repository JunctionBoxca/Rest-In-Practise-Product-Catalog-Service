using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class UpdatingRecentEventsFeedTests
    {
        [Test]
        public void WhenListOfEventsIsEmptyShouldTransitionToRequeryingEvents()
        {
            IEnumerable<Event> events = new Event[] {};
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.EmptyCurrentFeed();

            ITask start = new UpdatingRecentEventsFeed(recentEventsFeed, events);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (RequeryingEvents), end);
            Assert.AreEqual(recentEventsFeed, end.GetRecentEventsFeed());
        }

        [Test]
        public void WhenRecentEventsFeedHasReachedQuotaShouldTransitionToArchivingRecentEventsFeedWithRemainingEvents()
        {
            EventBuilder eventBuilder = new EventBuilder();

            Event event1 = eventBuilder.WithId(1).Build();
            Event event2 = eventBuilder.WithId(2).Build();
            Event event3 = eventBuilder.WithId(3).Build();
            
            EventBuffer buffer = new EventBufferBuilder().WithEvents(event1, event2, event3).Build();
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota - 1).Build();

            ITask start = new UpdatingRecentEventsFeed(recentEventsFeed, buffer.Take(QueryingEvents.BatchSize));
            
            ITask end = start.Execute(new InMemoryFileSystem(), buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (ArchivingRecentEventsFeed), end);
            Assert.AreEqual(recentEventsFeed, end.GetRecentEventsFeed());
            Assert.AreEqual(2, end.GetEvents().Count());
            Assert.AreEqual(event2, end.GetEvents().ElementAt(0));
            Assert.AreEqual(event3, end.GetEvents().ElementAt(1));
        }

        [Test]
        public void WhileThereAreEventsInTheListOfEventsAndTheFeedIsUnderQuotaShouldContinueAddingEventsToFeed()
        {
            EventBuilder builder = new EventBuilder();

            IEnumerable<Event> events = new[] {builder.Build(), builder.Build(), builder.Build()};
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(1).Build();

            ITask start = new UpdatingRecentEventsFeed(recentEventsFeed, events);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (RequeryingEvents), end);
            Assert.AreEqual(4, recentEventsFeed.GetNumberOfEntries());
        }

        [Test]
        public void WhenRecentEventsFeedHasReachedQuotaAndThereAreNoRemainingEventsShouldTransitionToRequeryingEvents()
        {
            EventBuffer buffer = new EventBufferBuilder().WithNumberOfEvents(1).Build();
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota - 1).Build();

            ITask start = new UpdatingRecentEventsFeed(recentEventsFeed, buffer.Take(QueryingEvents.BatchSize));
            
            ITask end = start.Execute(new InMemoryFileSystem(), buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (RequeryingEvents), end);
            Assert.AreEqual(recentEventsFeed, end.GetRecentEventsFeed());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new ArchivingRecentEventsFeed(RecentEventsFeedBuilder.FullCurrentFeed(), new Event[] { }).IsLastTask);
        }
    }
}