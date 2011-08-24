using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Shared.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class RewriteEntityBodyTests
    {
        [Test]
        public void ShouldRemoveViaLink()
        {
            RewriteEntityBody rewriteStrategy = new RewriteEntityBody(ResourceStreams.CurrentFeed());
            MemoryStream destination = new MemoryStream();

            rewriteStrategy.WriteEntityBody(destination);

            destination.Seek(0, SeekOrigin.Begin);
            using (XmlReader reader = XmlReader.Create(destination))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                Assert.IsNull(feed.GetViaLink());
            }
        }

        [Test]
        public void ShouldSetSelfLinkHrefValueToHrefValueOfViaLink()
        {
            string selfHref;
            string viaHref;

            using (XmlReader reader = XmlReader.Create(ResourceStreams.CurrentFeed()))
            {
                SyndicationFeed originalFeed = SyndicationFeed.Load(reader);
                selfHref = originalFeed.GetSelfLink().GetAbsoluteUri().AbsoluteUri;
                viaHref = originalFeed.GetViaLink().GetAbsoluteUri().AbsoluteUri;

                Assert.AreNotEqual(selfHref, viaHref);
            }

            RewriteEntityBody rewriteStrategy = new RewriteEntityBody(ResourceStreams.CurrentFeed());
            MemoryStream destination = new MemoryStream();

            rewriteStrategy.WriteEntityBody(destination);

            destination.Seek(0, SeekOrigin.Begin);
            using (XmlReader reader = XmlReader.Create(destination))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                Assert.AreEqual(viaHref, feed.GetSelfLink().GetAbsoluteUri().AbsoluteUri);
                Assert.AreNotEqual(selfHref, feed.GetSelfLink().GetAbsoluteUri().AbsoluteUri);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ObjectDisposedException))]
        public void ShouldCloseSourceStream()
        {
            FileStream source = new FileStream(@"..\..\Data\productcatalog\current.atom", FileMode.Open);

            IEntityBodyTransformationStrategy transformationStrategy = new RewriteEntityBody(source);

            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);

            source.Seek(0, SeekOrigin.Begin);
        }

        [Test]
        public void ShouldNotCloseDestinationStream()
        {
            IEntityBodyTransformationStrategy transformationStrategy = new RewriteEntityBody(ResourceStreams.CurrentFeed());

            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);

            Assert.AreEqual(0, destination.Seek(0, SeekOrigin.Begin));
        }
    }
}