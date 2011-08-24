using System;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class NotifyingListenersTests
    {
        [Test]
        public void ShouldNotifyListeners()
        {
            FeedMappingsChangedEventArgs args = null;
            Action<FeedMappingsChangedEventArgs> notifyListeners = a => args = a;

            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();

            ITask start = new NotifyingListeners(recentEventsFeed);
            start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), notifyListeners);

            Assert.AreEqual(args.RecentEventsFeedResourceId, recentEventsFeed.GetFeedMapping().Id.GetValue().ToString());
            Assert.AreEqual(args.RecentEventsFeedStoreId, recentEventsFeed.GetFeedMapping().FileName.GetValue());
        }

        [Test]
        public void ShouldTransitionToTerminating()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();

            ITask start = new NotifyingListeners(recentEventsFeed);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (Terminate), end);
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new NotifyingListeners(RecentEventsFeedBuilder.FullCurrentFeed()).IsLastTask);
        }
    }
}