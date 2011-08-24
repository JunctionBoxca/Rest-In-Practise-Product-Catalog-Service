using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class FeedMappingTests
    {
        [Test]
        public void NewFeedMappingShouldHaveTempFileName()
        {
            FeedMapping mapping = new FeedMapping(Id.InitialValue);

            Assert.AreEqual(Id.InitialValue, mapping.GetId());
            Assert.IsTrue(new Regex(@"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}.atom").IsMatch(mapping.GetFileName().GetValue()));
        }

        [Test]
        public void WithPermanentFileNameShouldReturnMappingWithFileNameDerivedFromId()
        {
            FeedMapping mapping = new FeedMapping(Id.InitialValue);
            FeedMapping withPermanentFileName = mapping.WithArchiveFileName();

            Assert.AreEqual(mapping.GetId(), withPermanentFileName.GetId());
            Assert.False(ReferenceEquals(mapping.GetId(), withPermanentFileName.GetId()));
            Assert.AreEqual(Path.ChangeExtension(mapping.GetId().GetValue().ToString(), FileName.Extension), withPermanentFileName.GetFileName().GetValue());
        }

        [Test]
        public void WithNextIdShouldReturnMappingWithNextIdAndNewFileName()
        {
            FeedMapping mapping = new FeedMapping(new Id(10));
            FeedMapping withNextId = mapping.WithNextId();

            Assert.AreNotEqual(mapping.GetFileName(), withNextId.GetFileName());
            Assert.AreEqual(new Id(11), withNextId.GetId());
        }
    }
}