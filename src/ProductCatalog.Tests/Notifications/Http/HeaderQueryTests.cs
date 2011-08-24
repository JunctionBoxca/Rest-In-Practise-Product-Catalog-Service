using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class HeaderQueryTests
    {
        [Test]
        public void WhenRepresentationContainsSpecifiedHeaderShouldReturnTrue()
        {
            IRepresentation representation = new HeadersOnlyRepresentation(ContentType.Atom);
            HeaderQuery query = new HeaderQuery(ContentType.Atom);
            Assert.IsTrue(query.Matches(representation));
        }

        [Test]
        public void WhenRepresentationDoesNotContainSpecifiedHeaderShouldReturnFalse()
        {
            IRepresentation representation = new HeadersOnlyRepresentation();
            HeaderQuery query = new HeaderQuery(CacheControl.ShortCachingPolicy);
            Assert.IsFalse(query.Matches(representation));
        }

        [Test]
        public void ShouldMatchHeaderInNestedRepresentation()
        {
            IRepresentation innerRepresentation = new HeadersOnlyRepresentation(new ETag("xyz"));
            IRepresentation representation = new HeadersOnlyRepresentation(innerRepresentation, ContentType.Atom);
            HeaderQuery query = new HeaderQuery(new ETag("xyz"));
            Assert.IsTrue(query.Matches(representation));
        }
    }
}