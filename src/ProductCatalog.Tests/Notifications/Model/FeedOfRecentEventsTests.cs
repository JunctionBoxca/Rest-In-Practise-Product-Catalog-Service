using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class FeedOfRecentEventsTests
    {
        [Test]
        public void ShouldAddShortCachingPolicyCacheControlHeader()
        {
            IRepresentation feedOfRecentEvents = new FeedOfRecentEvents(StreamBackedRepresentation.CreateCurrentFeed());
            Output output = Output.For(feedOfRecentEvents);
            Assert.AreEqual("max-age=10", output.CacheControl);
        }

        [Test]
        public void ShouldNotTransformEntityBody()
        {
            IRepresentation feedOfRecentEvents = new FeedOfRecentEvents(StreamBackedRepresentation.CreateCurrentFeed());
            Output output = Output.For(feedOfRecentEvents);
            Assert.AreEqual(output.EntityBody, StreamBackedRepresentation.CreateCurrentFeed().GetContents());
        }
    }
}