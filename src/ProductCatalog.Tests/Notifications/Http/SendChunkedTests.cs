using System;
using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class SendChunkedTests
    {
        [Test]
        public void WhenChunkingIsRequiredShouldNotWriteContentLengthButShouldSetIsChunkedToTrue()
        {
            IChunkingStrategy strategy = new SendChunked(true);
            IHeader header = strategy.CreateHeader(100);

            Output output = Output.For(header);

            Assert.IsNull(output.ContentLength);
            Assert.IsTrue(output.IsChunked.Value);
        }

        [Test]
        public void WhenChunkingIsNotRequiredShouldWriteContentLengthToResponseButShouldSetIsChunkedToFalse()
        {
            IChunkingStrategy strategy = new SendChunked(false);
            IHeader header = strategy.CreateHeader(100);

            Output output = Output.For(header);

            Assert.AreEqual(100, output.ContentLength);
            Assert.IsFalse(output.IsChunked.Value);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (InvalidOperationException))]
        public void WhenWritingToResponseWhenChunkingIsNotRequiredShouldThrowException()
        {
            IHeader header = new SendChunked(false);
            Output.For(header);
        }
    }
}