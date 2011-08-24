using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Service;

namespace ProductCatalog.Tests.Notifications.Service
{
    [TestFixture]
    public class NullFeedMappingsTests
    {
        [Test]
        [ExpectedException(typeof (ServerException))]
        public void CreateStoreAccessorForCurrentFeedThrowsException()
        {
            NullFeedMappings.Instance.CreateFeedOfRecentEventsAccessor();
        }

        [Test]
        [ExpectedException(typeof (ServerException))]
        public void CreateStoreAccessorThrowsException()
        {
            NullFeedMappings.Instance.CreateFeedAccessor(new ResourceId("resource-id"));
        }
    }
}