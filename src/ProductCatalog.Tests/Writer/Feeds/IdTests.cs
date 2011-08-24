using System;
using NUnit.Framework;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class IdTests
    {
        [Test]
        public void ShouldExhibitValueTypeEquality()
        {
            Id id1 = new Id(1);
            Id id2 = new Id(1);
            Id id3 = new Id(2);

            Assert.True(id1.Equals(id2));
            Assert.False(id1.Equals(id3));
            Assert.True(id1.Equals(id1));
            Assert.False(id1.Equals(new object()));
            Assert.False(id1.Equals(null));

            Assert.True(id1.GetHashCode().Equals(id2.GetHashCode()));
            Assert.False(id1.GetHashCode().Equals(id3.GetHashCode()));
            Assert.True(id1.GetHashCode().Equals(id1.GetHashCode()));
            Assert.False(id1.GetHashCode().Equals(new object().GetHashCode()));
        }

        [Test]
        public void ShouldBeAbleToCloneSelf()
        {
            Id id = new Id(1);
            Id clone = id.Clone();

            Assert.AreEqual(id, clone);
            Assert.False(ReferenceEquals(id, clone));
        }

        [Test]
        public void ShouldBeAbleToReturnFileNameDerivedFromValue()
        {
            Id id = new Id(10);
            FileName fileName = id.CreateFileName();

            Assert.AreEqual(new FileName("10.atom"), fileName);
        }

        [Test]
        public void ShouldBeAbleToReturnUriDerivedFromValue()
        {
            UriTemplate uriTemplate = new UriTemplate("/product-catalog/notifications/?page={id}");
            Uri baseAddress = new Uri("http://localhost/");

            Id id = new Id(10);

            Assert.AreEqual(new Uri("http://localhost/product-catalog/notifications/?page=10"), id.CreateUri(baseAddress, uriTemplate));
        }

        [Test]
        public void ToStringShouldReturnStringValueOfId()
        {
            Id id = new Id(10);
            Assert.AreEqual("10", id.ToString());
        }
    }
}