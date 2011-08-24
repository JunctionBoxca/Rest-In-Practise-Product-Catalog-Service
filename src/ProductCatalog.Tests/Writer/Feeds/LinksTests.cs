using System;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class LinksTests
    {
        private static readonly Links LinksUnderTest = new Links(new UriConfiguration(
            new Uri("http://restbucks.com/product-catalog/notifications/"), new UriTemplate("/recent"),
            new UriTemplate("/?page={id}"),
            new UriTemplate("/notification/{id}")));

        
        [Test]
        public void ShouldBeAbleToCreateRecentFeedSelfLink()
        {
            SyndicationLink link = LinksUnderTest.CreateRecentFeedSelfLink();
            Assert.AreEqual("self", link.RelationshipType);
            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/recent"), link.GetAbsoluteUri());
        }

        [Test]
        public void ShouldBeAbleToCreateViaLink()
        {
            SyndicationLink link = LinksUnderTest.CreateViaLink(new Id(5));
            Assert.AreEqual("via", link.RelationshipType);
            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/?page=5"), link.GetAbsoluteUri());
        }

        [Test]
        public void ShouldBeAbleToCreatePrevArchiveLink()
        {
            SyndicationLink link = LinksUnderTest.CreatePrevArchiveLink(new Id(5));
            Assert.AreEqual("prev-archive", link.RelationshipType);
            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/?page=5"), link.GetAbsoluteUri());
        }

        [Test]
        public void ShouldBeAbleToCreateNextArchiveLink()
        {
            SyndicationLink link = LinksUnderTest.CreateNextArchiveLink(new Id(5));
            Assert.AreEqual("next-archive", link.RelationshipType);
            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/?page=5"), link.GetAbsoluteUri());
        }

        [Test]
        public void ShouldBeAbleToCreateEntrySelfLink()
        {
            SyndicationLink link = LinksUnderTest.CreateEntrySelfLink(new Id(10));
            Assert.AreEqual("self", link.RelationshipType);
            Assert.AreEqual(new Uri("http://restbucks.com/product-catalog/notifications/notification/10"), link.GetAbsoluteUri());
        }
        
        [Test]
        public void ShouldBeAbleToParseResourceIdFromUri()
        {
            
            Id id = LinksUnderTest.GetIdFromFeedUri(new Uri("http://localhost/product-catalog/notifications/?page=6"));
            Assert.AreEqual(6, id.GetValue());
        }
    }
}