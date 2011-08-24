using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class RecentEventsFeedTests
    {
        [Test]
        public void ShouldCreateSyndicationFeed()
        {
            DateTimeOffset now = DateTime.Now;

            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithId(2).WithPreviousId(1).WithUris(SampleLinks.Instance).Build();
            SyndicationFeed syndicationFeed = recentEventsFeed.GetSyndicationFeed();

            Assert.True(new Regex(@"urn:uuid:\w{8}-\w{4}-\w{4}-\w{4}-\w{12}").IsMatch(syndicationFeed.Id));
            Assert.AreEqual("Restbucks products and promotions", syndicationFeed.Title.Text);
            Assert.AreEqual("Product Catalog", syndicationFeed.Authors.First().Name);
            Assert.AreEqual("Product Catalog", syndicationFeed.Generator);
            Assert.LessOrEqual(syndicationFeed.LastUpdatedTime.Subtract(now), new TimeSpan(0, 0, 2));
            Assert.AreEqual("http://restbucks.com/product-catalog/notifications/?page=2", syndicationFeed.GetViaLink().Uri.AbsoluteUri);
            Assert.AreEqual("http://restbucks.com/product-catalog/notifications/?page=1", syndicationFeed.GetPrevArchiveLink().Uri.AbsoluteUri);
            Assert.AreEqual("http://restbucks.com/product-catalog/notifications/recent", syndicationFeed.GetSelfLink().Uri.AbsoluteUri);
        }

        [Test]
        public void IfThereIsNoPreviousFeedTheFeedShouldNotIncludeAPreviousArchiveLink()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.WithoutPreviousFeed();
            Assert.IsNull(recentEventsFeed.GetSyndicationFeed().GetPrevArchiveLink());
        }

        [Test]
        public void ShouldSaveFeedToCurrentDirectory()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();

            RecentEventsFeed recentEventsFeed = new RecentEventsFeedBuilder().WithId(2).WithPreviousId(1).WithUris(SampleLinks.Instance).Build();

            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.CurrentDirectory));

            recentEventsFeed.Save(fileSystem);

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));

            SyndicationFeed feed = recentEventsFeed.GetSyndicationFeed();
            SyndicationFeed rehydratedFeed = SyndicationFeed.Load(fileSystem.CurrentDirectory.GetXmlReader(recentEventsFeed.GetFeedMapping().GetFileName()));

            Assert.AreEqual(feed.Id, rehydratedFeed.Id);
            Assert.AreEqual(feed.Items.Count(), rehydratedFeed.Items.Count());
            Assert.AreEqual(feed.GetSelfLink().GetAbsoluteUri(), rehydratedFeed.GetSelfLink().GetAbsoluteUri());
            Assert.AreEqual(feed.GetViaLink().GetAbsoluteUri(), rehydratedFeed.GetViaLink().GetAbsoluteUri());
            Assert.AreEqual(feed.GetPrevArchiveLink().GetAbsoluteUri(), rehydratedFeed.GetPrevArchiveLink().GetAbsoluteUri());
        }

        [Test]
        public void WhenArchivingShouldAddANextArchiveLink()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();
            RecentEventsFeed nextFeed = recentEventsFeed.CreateNextFeed();
            ArchiveFeed archive = recentEventsFeed.CreateArchiveFeed(nextFeed);

            Uri nextSelfUri = nextFeed.GetSyndicationFeed().GetViaLink().GetAbsoluteUri();
            Uri nextArchiveUri = archive.GetSyndicationFeed().GetNextArchiveLink().GetAbsoluteUri();

            Assert.AreEqual(nextSelfUri, nextArchiveUri);
        }

        [Test]
        public void WhenArchivingShouldSetSelfLinkToValueOfFormerViaLink()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();
            RecentEventsFeed nextFeed = recentEventsFeed.CreateNextFeed();
            ArchiveFeed archive = recentEventsFeed.CreateArchiveFeed(nextFeed);

            Uri viaUri = recentEventsFeed.GetSyndicationFeed().GetViaLink().GetAbsoluteUri();
            Uri formerSelfUri = archive.GetSyndicationFeed().GetSelfLink().GetAbsoluteUri();

            Assert.AreEqual(viaUri, formerSelfUri);
        }

        [Test]
        public void WhenArchivingShouldRemoveViaLink()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();
            RecentEventsFeed nextFeed = recentEventsFeed.CreateNextFeed();
            ArchiveFeed archive = recentEventsFeed.CreateArchiveFeed(nextFeed);

            Assert.IsNull(archive.GetSyndicationFeed().GetViaLink());
        }

        [Test]
        public void WhenArchivingShouldAddArchiveSimpleExtensionElement()
        {
            RecentEventsFeed recentEventsFeed = RecentEventsFeedBuilder.FullCurrentFeed();
            RecentEventsFeed nextFeed = recentEventsFeed.CreateNextFeed();
            ArchiveFeed archive = recentEventsFeed.CreateArchiveFeed(nextFeed);

            Func<SyndicationElementExtension, bool> isArchiveElement = el => el.OuterName.Equals("archive") &&
                                                                             el.OuterNamespace.Equals("http://purl.org/syndication/history/1.0") &&
                                                                             el.GetReader().Value.Equals(string.Empty);

            Assert.IsNotNull(archive.GetSyndicationFeed().ElementExtensions.Single(isArchiveElement));
        }

        [Test]
        public void WhenAnEntryIsAddedToTheFeedTheUpdatedElementOnTheFeedShouldBeUpdated()
        {
            RecentEventsFeed feed = new RecentEventsFeedBuilder().Build();
            DateTimeOffset initialDateTime = feed.GetSyndicationFeed().LastUpdatedTime;

            Thread.Sleep(1000);

            feed.AddEvent(new EventBuilder().Build());
            DateTimeOffset latestDateTime = feed.GetSyndicationFeed().LastUpdatedTime;

            Assert.Greater(latestDateTime, initialDateTime);
        }
    }
}