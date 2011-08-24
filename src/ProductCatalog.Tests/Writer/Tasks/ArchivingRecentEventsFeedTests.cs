using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class ArchivingRecentEventsFeedTests
    {
        [Test]
        public void ShouldSaveArchivedVersionOfRecentEventsFeedToArchiveDirectory()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            Assert.False(fileSystem.FileExists(fileSystem.ArchiveDirectory, new FileName("11.atom")));

            IEnumerable<Event> events = new [] { new EventBuilder().Build()};
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota).WithId(11).Build();

            ITask start = new ArchivingRecentEventsFeed(recentEventsFeed, events);
            start.Execute(fileSystem, new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.True(fileSystem.FileExists(fileSystem.ArchiveDirectory, new FileName("11.atom")));
        }

        [Test]
        public void ShouldTransitionToUpdatingRecentEventsFeedWithNewRecentEventsFeed()
        {
            IEnumerable<Event> events = new[] { new EventBuilder().Build() };
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota).WithId(11).Build();

            ITask start = new ArchivingRecentEventsFeed(recentEventsFeed, events);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (UpdatingRecentEventsFeed), end);

            Assert.AreEqual(new Id(12), end.GetRecentEventsFeed().GetFeedMapping().GetId());
            Assert.AreEqual(1, end.GetEvents().Count());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new ArchivingRecentEventsFeed(RecentEventsFeedBuilder.FullCurrentFeed(), new Event[]{}).IsLastTask);
        }
    }
}