using NUnit.Framework;
using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class ResponseContextTests
    {
        [Test]
        public void IfSuppliedHeaderMatchesContainedHeaderShouldReturnTrue()
        {
            IResponseContext context = new ResponseContext();
            context.AddHeader(new ETag("123"));
            context.AddHeader(ContentType.Atom);

            Assert.True(context.ContainsHeader(new ETag("123")));
            Assert.True(context.ContainsHeader(ContentType.Atom));

            Assert.False(context.ContainsHeader(CacheControl.LongCachingPolicy));
            Assert.False(context.ContainsHeader(new ETag("abc")));
        }
    }
}