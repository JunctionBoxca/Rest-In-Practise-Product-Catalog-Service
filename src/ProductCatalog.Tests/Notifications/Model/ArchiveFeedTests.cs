using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class ArchiveFeedTests
    {
        [Test]
        public void ShouldAddLongCachingPolicyCacheControlHeader()
        {
            IRepresentation archiveFeed = new ArchiveFeed(StreamBackedRepresentation.CreateArchiveFeed());
            Output output = Output.For(archiveFeed);
            Assert.AreEqual("max-age=10000", output.CacheControl);
        }

        [Test]
        public void ShouldNotTransformEntityBody()
        {
            IRepresentation archiveFeed = new ArchiveFeed(StreamBackedRepresentation.CreateArchiveFeed());
            Output output = Output.For(archiveFeed);
            Assert.AreEqual(output.EntityBody, StreamBackedRepresentation.CreateArchiveFeed().GetContents());
        }
    }
}