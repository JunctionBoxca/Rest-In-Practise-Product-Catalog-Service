using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class NullRepresentationTests
    {
        [Test]
        public void ShouldNotAddAnythingToResponse()
        {
            Output output = Output.For(NullRepresentation.Instance);

            Assert.IsNull(output.IsChunked);
            Assert.IsNull(output.StatusCode);
            Assert.IsNull(output.StatusDescription);
            Assert.IsNull(output.CacheControl);
            Assert.IsNull(output.ContentLength);
            Assert.IsNull(output.ContentType);
            Assert.IsNull(output.ETag);
            Assert.IsNull(output.LastModified);
            Assert.IsNull(output.EntityBody);
        }
    }
}