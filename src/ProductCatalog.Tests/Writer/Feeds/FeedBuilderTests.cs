using System;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;

namespace ProductCatalog.Tests.Writer.Feeds
{
    public class FeedBuilderTests
    {
        [Test]
        public void ShouldBeAbleToLoadFeedFromCurrentDirectory()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();

            FeedBuilder builder = new FeedBuilder(SampleLinks.Instance);
            FeedMapping mapping = new FeedMapping(Id.InitialValue);
            RecentEventsFeed feed = builder.CreateRecentEventsFeed(mapping, PrevArchiveLinkGenerator.Null);

            feed.Save(fileSystem);

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.CurrentDirectory));

            RecentEventsFeed copy = builder.LoadRecentEventsFeed(fileSystem, mapping.FileName);

            Assert.AreEqual(feed.GetSyndicationFeed().Id, copy.GetSyndicationFeed().Id);
        }

        [Test]
        public void GivenAnEventShouldCreateEntryRepresentingThatEvent()
        {
            FeedBuilder builder = new FeedBuilder(SampleLinks.Instance);

            Event evnt = new EventBuilder().WithId(1).WithTimestamp(new DateTime(2008, 8, 12)).Build();

            Entry entry = builder.CreateEntry(evnt);

            SyndicationItem item = entry.GetSyndicationItem();

            Assert.AreEqual("tag:restbucks.com,2008-08-12:1", item.Id);
            Assert.AreEqual(evnt.Subject, item.Title.Text);
            Assert.AreEqual(new DateTimeOffset(evnt.Timestamp), item.LastUpdatedTime);
            Assert.AreEqual(evnt.Body.ContentType, item.Content.Type);
            Assert.AreEqual(evnt.Body.Href, item.GetRelatedLink().Uri.AbsoluteUri);
            Assert.AreEqual("http://restbucks.com/product-catalog/notifications/notification/1", item.GetSelfLink().Uri.AbsoluteUri);
        }
    }
}