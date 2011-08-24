using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer
{
    [TestFixture]
    public class FeedWriterTests
    {
        [Test]
        public void WhenTriggeredShouldAddBatchOfEventsToCurrentFeed()
        {
            FileName tempFileName = null;

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();

            EventBuilder eventBuilder = new EventBuilder();
            Repeat.Times(2).Action(() => buffer.Add(eventBuilder.Build()));

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => tempFileName = new FileName(args.RecentEventsFeedStoreId));

            timer.Fire();

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));

            RecentEventsFeed rehydratedFeed = new FeedBuilder(SampleLinks.Instance).LoadRecentEventsFeed(fileSystem, tempFileName);
            Assert.AreEqual(2, rehydratedFeed.GetNumberOfEntries());
        }

        [Test]
        public void WhenThereAreNoOutstandingEventsShouldRetainTheCurrentTempFileName()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();
            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));

            Repeat.Times(1).Action(() => buffer.Add(new EventBuilder().Build()));

            timer.Fire();

            Repeat.Times(0).Action(() => buffer.Add(new EventBuilder().Build()));

            timer.Fire();

            Repeat.Times(1).Action(() => buffer.Add(new EventBuilder().Build()));

            timer.Fire();

            Assert.AreEqual(2, fileSystem.FileCount(fileSystem.CurrentDirectory));
        }

        [Test]
        public void WhenNumberOfEventsExceedsQuotaShouldArchiveFeedAndBeginAnotherOne()
        {
            FileName tempFileName = null;
            FeedBuilder feedBuilder = new FeedBuilder(SampleLinks.Instance);

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();

            EventBuilder eventBuilder = new EventBuilder();
            Repeat.Times(RecentEventsFeed.Quota + 1).Action(() => buffer.Add(eventBuilder.Build()));

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => tempFileName = new FileName(args.RecentEventsFeedStoreId));

            timer.Fire();

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));
            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.ArchiveDirectory));

            ArchiveFeed archivedFeed = LoadArchiveFeed(fileSystem, new FileName("1"));
            Assert.AreEqual(10, archivedFeed.GetNumberOfEntries());
            Assert.AreEqual("?page=1", archivedFeed.GetSyndicationFeed().GetSelfLink().Uri.Query);
            Assert.AreEqual("?page=2", archivedFeed.GetSyndicationFeed().GetNextArchiveLink().Uri.Query);

            RecentEventsFeed recentEventsFeed = feedBuilder.LoadRecentEventsFeed(fileSystem, tempFileName);
            Assert.AreEqual(1, recentEventsFeed.GetSyndicationFeed().Items.Count());
            Assert.AreEqual("?page=2", recentEventsFeed.GetSyndicationFeed().GetViaLink().Uri.Query);
        }

        public ArchiveFeed LoadArchiveFeed(IFileSystem fileSystem, FileName fileName)
        {
            using (XmlReader reader = fileSystem.ArchiveDirectory.GetXmlReader(fileName))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                Id id = SampleLinks.Instance.GetIdFromFeedUri(feed.GetSelfLink().GetAbsoluteUri());
                return new ArchiveFeed(feed, new FeedMapping(id));
            }
        }

        [Test]
        public void WhenTriggeredRepeatedlyShouldContinueToAddEventsToExistingFeed()
        {
            FileName tempFileName = null;
            FeedBuilder feedBuilder = new FeedBuilder(SampleLinks.Instance);

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();

            buffer.Add(new EventBuilder().Build());

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => tempFileName = new FileName(args.RecentEventsFeedStoreId));

            timer.Fire();

            RecentEventsFeed rehydratedFeed = feedBuilder.LoadRecentEventsFeed(fileSystem, tempFileName);
            Assert.AreEqual(1, rehydratedFeed.GetSyndicationFeed().Items.Count());
            Assert.AreEqual("?page=1", rehydratedFeed.GetSyndicationFeed().GetViaLink().Uri.Query);

            buffer.Add(new EventBuilder().Build());

            timer.Fire();

            RecentEventsFeed secondRehydratedFeed = feedBuilder.LoadRecentEventsFeed(fileSystem, tempFileName);
            Assert.AreEqual(2, secondRehydratedFeed.GetSyndicationFeed().Items.Count());
            Assert.AreEqual("?page=1", secondRehydratedFeed.GetSyndicationFeed().GetViaLink().Uri.Query);
        }

        [Test]
        public void ShouldRaiseEventEachTimeATempFileIsSavedToCurrentDirectory()
        {
            int eventCount = 0;

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBufferBuilder().Build();
            EventBuilder eventBuilder = new EventBuilder();

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => eventCount++);

            //Current will contain 1 - 1st version of 1 will be saved to current
            Repeat.Times(1).Action(() => buffer.Add(eventBuilder.Build()));

            timer.Fire();

            //Current will contain full feed - 2nd version of 1 will be saved to current
            Repeat.Times(RecentEventsFeed.Quota - 1).Action(() => buffer.Add(eventBuilder.Build()));

            timer.Fire();

            //Feed will be archived 3 times - 1st version of 4 will be saved to current
            Repeat.Times((RecentEventsFeed.Quota * 3) - 1).Action(() => buffer.Add(eventBuilder.Build()));

            timer.Fire();

            Assert.AreEqual(3, eventCount);

            Assert.AreEqual(3, fileSystem.FileCount(fileSystem.CurrentDirectory));
            Assert.AreEqual(3, fileSystem.FileCount(fileSystem.ArchiveDirectory));

            Assert.IsTrue(fileSystem.FileExists(fileSystem.ArchiveDirectory, new FileName("1")));
            Assert.IsTrue(fileSystem.FileExists(fileSystem.ArchiveDirectory, new FileName("2")));
            Assert.IsTrue(fileSystem.FileExists(fileSystem.ArchiveDirectory, new FileName("3")));
        }

        [Test]
        public void WhenThereAreNoOutstandingEventsShouldNotWriteFeedOrRaiseEvent()
        {
            int eventCount = 0;

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => eventCount++);

            timer.Fire();

            Assert.AreEqual(0, eventCount);
            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.CurrentDirectory));
        }

        [Test]
        public void IfFeedFillsExactlyToQuotaShouldNotBeArchived()
        {
            FileName tempFileName = null;
            FeedBuilder feedBuilder = new FeedBuilder(SampleLinks.Instance);

            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            EventBuffer buffer = new EventBuffer();

            Repeat.Times(3).Action(() => buffer.Add(new EventBuilder().Build()));

            FakeTimer timer = new FakeTimer();
            FeedWriter feedWriter = new FeedWriter(timer, buffer, fileSystem, new FeedBuilder(SampleLinks.Instance));
            feedWriter.FeedMappingsChanged += ((o, args) => tempFileName = new FileName(args.RecentEventsFeedStoreId));

            timer.Fire();

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));
            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.ArchiveDirectory));

            RecentEventsFeed rehydratedFeed = feedBuilder.LoadRecentEventsFeed(fileSystem, tempFileName);
            Assert.AreEqual(3, rehydratedFeed.GetSyndicationFeed().Items.Count());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "timer cannot be null.")]
        public void IfTimerIsNullShouldThrowException()
        {
            new FeedWriter(null, new EventBuffer(), new InMemoryFileSystem(), new FeedBuilder(SampleLinks.Instance));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "buffer cannot be null.")]
        public void IfEventBufferIsNullShouldThrowException()
        {
            new FeedWriter(new FakeTimer(), null, new InMemoryFileSystem(), new FeedBuilder(SampleLinks.Instance));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "fileSystem cannot be null.")]
        public void IfFileSystemIsNullShouldThrowException()
        {
            new FeedWriter(new FakeTimer(), new EventBuffer(), null, new FeedBuilder(SampleLinks.Instance));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "feedBuilder cannot be null.")]
        public void IfFeedBuilderIsNullShouldThrowException()
        {
            new FeedWriter(new FakeTimer(), new EventBuffer(), new InMemoryFileSystem(), null);
        }
    }
}