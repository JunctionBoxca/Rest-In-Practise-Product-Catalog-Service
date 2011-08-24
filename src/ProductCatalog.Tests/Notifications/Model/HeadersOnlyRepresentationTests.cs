using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class HeadersOnlyRepresentationTests
    {
        [Test]
        public void AddsHeadersToResponse()
        {
            IRepresentation representation = new HeadersOnlyRepresentation(new ETag("123"), ContentType.Atom);
            Output output = Output.For(representation);

            Assert.AreEqual("123", output.ETag);
            Assert.AreEqual("application/atom+xml", output.ContentType);
        }

        [Test]
        public void CallsInnerRepresentation()
        {
            IRepresentation innerRepresentation = new HeadersOnlyRepresentation(ContentType.Atom);
            IRepresentation representation = new HeadersOnlyRepresentation(innerRepresentation, new ETag("123"));
            Output output = Output.For(representation);

            Assert.AreEqual("123", output.ETag);
            Assert.AreEqual("application/atom+xml", output.ContentType);
        }
    }
}