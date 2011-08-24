using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class ContentTypeTests
    {
        [Test]
        public void AtomFeedShouldAddAtomContentTypeHeaderWithFeedTypeParameter()
        {
            Output output = Output.For(ContentType.Atom);
            Assert.AreEqual("application/atom+xml", output.ContentType);
        }
    }
}