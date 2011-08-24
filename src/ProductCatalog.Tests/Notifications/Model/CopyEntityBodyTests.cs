using System;
using System.IO;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Shared.Utility;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class CopyEntityBodyTests
    {
        [Test]
        public void ShouldCopyEntityBodyWithoutTransformingContents()
        {
            IEntityBodyTransformationStrategy transformationStrategy = new CopyEntityBody(ResourceStreams.CurrentFeed());

            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);

            string sourceContents;
            string destinationContents;

            using (StreamReader reader = new StreamReader(ResourceStreams.CurrentFeed()))
            {
                sourceContents = reader.ReadToEnd();
            }

            destination.Seek(0, SeekOrigin.Begin);

            using (StreamReader reader = new StreamReader(destination))
            {
                destinationContents = reader.ReadToEnd();
            }

            Assert.False(string.IsNullOrEmpty(sourceContents));
            Assert.False(string.IsNullOrEmpty(destinationContents));
            Assert.AreEqual(sourceContents, destinationContents);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (ObjectDisposedException))]
        public void ShouldCloseSourceStream()
        {
            FileStream source = new FileStream(@"..\..\Data\productcatalog\current.atom", FileMode.Open);

            IEntityBodyTransformationStrategy transformationStrategy = new CopyEntityBody(source);

            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);

            source.Seek(0, SeekOrigin.Begin);
        }

        [Test]
        public void ShouldNotCloseDestinationStream()
        {
            IEntityBodyTransformationStrategy transformationStrategy = new CopyEntityBody(ResourceStreams.CurrentFeed());

            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);

            Assert.AreEqual(0, destination.Seek(0, SeekOrigin.Begin));
        }
    }
}