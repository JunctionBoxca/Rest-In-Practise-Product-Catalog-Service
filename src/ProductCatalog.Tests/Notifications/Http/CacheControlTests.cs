using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class CacheControlTests
    {
        [Test]
        public void LongCachingPolicyShouldSetLongMaxAgeCacheControlHeader()
        {
            Output output = Output.For(CacheControl.LongCachingPolicy);
            Assert.AreEqual("max-age=10000", output.CacheControl);
        }

        [Test]
        public void ShortCachingPolicyShouldSetShortMaxAgeCacheControlHeader()
        {
            Output output = Output.For(CacheControl.ShortCachingPolicy);
            Assert.AreEqual("max-age=10", output.CacheControl);
        }
    }
}