using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Feeds
{
    [TestFixture]
    public class EntryTests
    {
        [Test]
        public void ShouldAddSelfToTopOfFeed()
        {
            SyndicationFeed feed = new SyndicationFeed();

            Entry entry1 = new Entry(new SyndicationItem {Id = "id1"}, new FileName("1"));
            Entry entry2 = new Entry(new SyndicationItem {Id = "id2"}, new FileName("2"));

            entry1.AddTo(feed);
            entry2.AddTo(feed);

            Assert.AreEqual(2, feed.Items.Count());

            Assert.AreEqual("id2", feed.Items.ElementAt(0).Id);
            Assert.AreEqual("id1", feed.Items.ElementAt(1).Id);
        }

        [Test]
        public void ShouldSaveEntryToEntryDirectory()
        {
            InMemoryFileSystem fileSystem = new InMemoryFileSystem();
            Entry entry = new Entry(new SyndicationItem {Id = "id1"}, new FileName("1"));

            Assert.AreEqual(0, fileSystem.FileCount(fileSystem.EntryDirectory));

            entry.Save(fileSystem);

            Assert.AreEqual(1, fileSystem.FileCount(fileSystem.EntryDirectory));

            SyndicationItem rehydratedItem = SyndicationItem.Load(fileSystem.EntryDirectory.GetXmlReader(new FileName("1")));

            Assert.AreEqual(rehydratedItem.Id, entry.GetSyndicationItem().Id);
        }
    }
}