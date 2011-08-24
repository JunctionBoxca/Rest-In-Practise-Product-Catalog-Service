using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class WorkingFeedTests
    {
        [Test]
        public void ShouldAddShortCachingPolicyCacheControlHeader()
        {
            IRepresentation workingFeed = new WorkingFeed(StreamBackedRepresentation.CreateCurrentFeed());
            Output output = Output.For(workingFeed);
            Assert.AreEqual("max-age=10", output.CacheControl);
        }

        [Test]
        public void ShouldTransformLinksInEntityBody()
        {
            IRepresentation workingFeed = new WorkingFeed(StreamBackedRepresentation.CreateCurrentFeed());
            Output output = Output.For(workingFeed);

            SyndicationFeed originalFeed = StreamBackedRepresentation.CreateCurrentFeed().ToSyndicationFeed();
            SyndicationFeed outputFeed = output.ToSyndicationFeed();

            Assert.IsNull(outputFeed.GetViaLink());
            Assert.AreEqual(originalFeed.GetViaLink().GetAbsoluteUri(), outputFeed.GetSelfLink().GetAbsoluteUri());
        }
    }
}