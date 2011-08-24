using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class IfNoneMatchTests
    {
        [Test]
        public void IfETagMatchesShouldReturnNotModified()
        {
            IRepresentation representation = new HeadersOnlyRepresentation(new ETag("etag"));

            IfNoneMatch condition = new IfNoneMatch(new ETag("etag"));
            IResponse response = condition.CreateResponse(representation);

            Output output = Output.For(response);

            Assert.AreEqual(304, output.StatusCode);
            Assert.AreEqual("Not Modified", output.StatusDescription);
            Assert.AreEqual("etag", output.ETag);
        }

        [Test]
        public void IfETagDoesNotMatchShouldReturnOK()
        {
            IRepresentation representation = new HeadersOnlyRepresentation(new ETag("different-etag"));

            IfNoneMatch condition = new IfNoneMatch(new ETag("etag"));
            IResponse response = condition.CreateResponse(representation);

            Output output = Output.For(response);

            Assert.AreEqual(200, output.StatusCode);
            Assert.AreEqual("OK", output.StatusDescription);
            Assert.AreEqual("different-etag", output.ETag);
        }
    }
}