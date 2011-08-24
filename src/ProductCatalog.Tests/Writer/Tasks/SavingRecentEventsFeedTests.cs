using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class SavingRecentEventsFeedTests
    {
        [Test]
        public void ShouldSaveRecentEventsFeedToCurrentDirectory()
        {
            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota - 1).WithId(11).Build();

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.CurrentDirectory));

            ITask start = new SavingRecentEventsFeed(recentEventsFeed);
            start.Execute(fileSystem, new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));
        }

        [Test]
        public void ShouldTransitionToNotifyingListeners()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();

            ITask start = new SavingRecentEventsFeed(recentEventsFeed);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (NotifyingListeners), end);
            Assert.AreEqual(recentEventsFeed, end.GetRecentEventsFeed());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new ArchivingRecentEventsFeed(RecentEventsFeedBuilder.FullCurrentFeed(), new Event[] {}).IsLastTask);
        }
    }
}