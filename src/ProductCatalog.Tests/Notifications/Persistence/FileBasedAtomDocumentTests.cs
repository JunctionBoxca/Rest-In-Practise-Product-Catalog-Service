using System.IO;
using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Persistence
{
    [TestFixture]
    public class FileBasedAtomDocumentTests
    {
        [Test]
        public void ShouldWriteETagHeaderToResponse()
        {
            string fileName = @"..\..\Data\productcatalog\current.atom";

            IRepresentation representation = new FileBasedAtomDocument(fileName, new SendChunked(true));

            Output output = Output.For(representation);

            FileInfo fileInfo = new FileInfo(fileName);
            string expectedETag = string.Format(@"""current.atom#{0}""", fileInfo.LastWriteTimeUtc.Ticks);

            Assert.AreEqual(expectedETag, output.ETag);
        }

        [Test]
        public void WhenSendChunkedIsFalseShouldWriteLengthHeaderToResponse()
        {
            string fileName = @"..\..\Data\productcatalog\current.atom";

            IRepresentation representation = new FileBasedAtomDocument(fileName, new SendChunked(false));

            Output output = Output.For(representation);

            FileInfo fileInfo = new FileInfo(fileName);

            Assert.AreEqual(fileInfo.Length, output.ContentLength);
            Assert.IsFalse(output.IsChunked.Value);
        }

        [Test]
        public void WhenSendChunkedIsTrueShouldNotWriteLengthHeaderToResponse()
        {
            string fileName = @"..\..\Data\productcatalog\current.atom";

            IRepresentation representation = new FileBasedAtomDocument(fileName, new SendChunked(true));

            Output output = Output.For(representation);

            Assert.IsNull(output.ContentLength);
            Assert.IsTrue(output.IsChunked.Value);
        }

        [Test]
        public void ShouldWriteLastUpdatedDateTimeToResponse()
        {
            string fileName = @"..\..\Data\productcatalog\current.atom";

            IRepresentation representation = new FileBasedAtomDocument(fileName, new SendChunked(true));

            Output output = Output.For(representation);

            FileInfo fileInfo = new FileInfo(fileName);

            Assert.AreEqual(fileInfo.LastWriteTimeUtc.ToString("R"), output.LastModified);
        }

        [Test]
        public void ShouldWriteAtomContentTypeHeaderToResponse()
        {
            string fileName = @"..\..\Data\productcatalog\current.atom";

            IRepresentation representation = new FileBasedAtomDocument(fileName, new SendChunked(true));

            Output output = Output.For(representation);

            Assert.AreEqual("application/atom+xml", output.ContentType);
        }

        [Test]
        [ExpectedException(typeof (NotFoundException))]
        public void IfFileDoesNotExistThrowsException()
        {
            new FileBasedAtomDocument("dhsgdhjsahdjasdjksahdasd.vgd", new SendChunked(true));
        }
    }
}