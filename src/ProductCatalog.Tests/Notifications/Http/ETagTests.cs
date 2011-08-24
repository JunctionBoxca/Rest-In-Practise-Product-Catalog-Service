using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class ETagTests
    {
        [Test]
        public void ShouldWriteETagValueToHeader()
        {
            Output output = Output.For(new ETag("xyz"));
            Assert.AreEqual("xyz", output.ETag);
        }

        [Test]
        public void ShouldExhibitValueTypeEquality()
        {
            ETag eTag1 = new ETag("A");
            ETag eTag2 = new ETag("A");
            ETag eTag3 = new ETag("B");

            Assert.True(eTag1.Equals(eTag2));
            Assert.False(eTag1.Equals(eTag3));
            Assert.True(eTag1.Equals(eTag1));
            Assert.False(eTag1.Equals(new object()));
            Assert.False(eTag1.Equals(null));
            Assert.False(eTag1.Equals(null as object));

            Assert.True(eTag1.GetHashCode().Equals(eTag2.GetHashCode()));
            Assert.False(eTag1.GetHashCode().Equals(eTag3.GetHashCode()));
            Assert.True(eTag1.GetHashCode().Equals(eTag1.GetHashCode()));
            Assert.False(eTag1.GetHashCode().Equals(new object().GetHashCode()));
        }
    }
}