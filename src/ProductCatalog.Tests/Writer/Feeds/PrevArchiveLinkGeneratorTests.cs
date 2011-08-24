using System;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class PrevArchiveLinkGeneratorTests
    {
        private static readonly Links Links = new Links(new UriConfiguration(
            new Uri("http://restbucks.com/product-catalog/notifications/"), new UriTemplate("/recent"),
            new UriTemplate("/?page={id}"),
            new UriTemplate("/notification/{id}")));

        [Test]
        public void CanAddPrevArchiveLinkToFeed()
        {
            SyndicationFeed feed = new SyndicationFeed("title", "description", new Uri("http://localhost/alternate"));

            IPrevArchiveLinkGenerator linkGenerator = new PrevArchiveLinkGenerator(new Id(15));
            linkGenerator.AddTo(feed, Links);

            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/?page=15"), feed.GetPrevArchiveLink().GetAbsoluteUri());
        }

        [Test]
        public void NullGeneratorDoesNotAddPrevArchiveLinkToFeed()
        {
            SyndicationFeed feed = new SyndicationFeed("title", "description", new Uri("http://localhost/alternate"));

            IPrevArchiveLinkGenerator linkGenerator = PrevArchiveLinkGenerator.Null;
            linkGenerator.AddTo(feed, Links);

            Assert.IsNull(feed.GetPrevArchiveLink());
        }
    }
}