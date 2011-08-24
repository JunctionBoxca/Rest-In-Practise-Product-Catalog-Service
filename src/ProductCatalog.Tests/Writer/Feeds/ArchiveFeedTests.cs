using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class ArchiveFeedTests
    {
        [Test]
        public void ShouldSaveFeedToArchiveDirectory()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();

            RecentEventsFeed recentEvents = new RecentEventsFeedBuilder().WithId(2).WithPreviousId(1).WithNumberOfEntries(5).Build();
            ArchiveFeed archive = recentEvents.CreateArchiveFeed(recentEvents.CreateNextFeed());

            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.ArchiveDirectory));

            archive.Save(fileSystem);

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.ArchiveDirectory));

            SyndicationFeed feed = archive.GetSyndicationFeed();

            FileName archiveFileName = recentEvents.GetFeedMapping().WithArchiveFileName().FileName;
            SyndicationFeed rehydrated = SyndicationFeed.Load(fileSystem.ArchiveDirectory.GetXmlReader(archiveFileName));

            Assert.AreEqual(feed.Id, rehydrated.Id);
            Assert.AreEqual(feed.Items.Count(), rehydrated.Items.Count());
            Assert.AreEqual(feed.GetSelfLink().GetAbsoluteUri(), rehydrated.GetSelfLink().GetAbsoluteUri());
            Assert.AreEqual(feed.GetPrevArchiveLink().GetAbsoluteUri(), rehydrated.GetPrevArchiveLink().GetAbsoluteUri());
            Assert.AreEqual(feed.GetNextArchiveLink().GetAbsoluteUri(), rehydrated.GetNextArchiveLink().GetAbsoluteUri());
        }
    }
}