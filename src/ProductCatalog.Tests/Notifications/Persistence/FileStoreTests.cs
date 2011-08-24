using System;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Persistence
{
    [TestFixture]
    public class FileStoreTests
    {
        private static readonly FileSystemConfiguration FileSystemConfiguration = 
            new FileSystemConfiguration(@"..\..\Data\");

        [Test]
        public void ShouldRetrieveCurrentFeedBasedOnStoreId()
        {
            IStoreId id = new StoreId<string>("current.atom");

            IStore store = new FileStore(FileSystemConfiguration);
            IRepresentation representation = store.GetCurrentFeed(id);

            Output output = Output.For(representation);

            Assert.AreEqual("http://restbucks.com/product-catalog/notifications/recent", GetSelfLinkValue(output.EntityBody));
        }

        [Test]
        public void ShouldRetrieveArchiveFeedBasedOnStoreId()
        {
            IStoreId id = new StoreId<string>("archive.atom");

            IStore store = new FileStore(FileSystemConfiguration);
            IRepresentation representation = store.GetArchiveFeed(id);

            Output output = Output.For(representation);

            Assert.AreEqual("http://restbucks.com/product-catalogue/archive", GetSelfLinkValue(output.EntityBody));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "fileSystemConfiguration cannot be null.")]
        public void IfFileSystemConfigurationIsNullShouldThrowException()
        {
            new FileStore(null);
        }


        private string GetSelfLinkValue(string entityBody)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(entityBody);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("a", "http://www.w3.org/2005/Atom");
            return xml.SelectSingleNode("//a:link[@rel='self']/@href", namespaceManager).Value;
        }
    }
}