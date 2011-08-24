using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Tasks
{
    [TestFixture]
    public class IdentifyingRecentEventsFeedTests
    {
        [Test]
        public void IfRecentEventsFeedExistsOnFileSystemTransitionsToUpdatingRecentEventsFeed()
        {
            int eventCount = 2;

            IFileSystem fileSystem = new InMemoryFileSystem();
            SyndicationFeed feed = SyndicationFeeds.Current();

            using (XmlWriter writer = fileSystem.CurrentDirectory.GetXmlWriter(FileName.TempFileName()))
            {
                feed.SaveAsAtom10(writer);
            }

            EventBuffer buffer = new EventBufferBuilder().WithNumberOfEvents(eventCount).Build();

            ITask start = new IdentifyingRecentEventsFeed(buffer.Take(QueryingEvents.BatchSize));
            ITask end = start.Execute(fileSystem, buffer, new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (UpdatingRecentEventsFeed), end);
            Assert.AreEqual(feed.Id, end.GetRecentEventsFeed().GetSyndicationFeed().Id);
            Assert.AreEqual(2, end.GetEvents().Count());
        }

        [Test]
        public void IfRecentEventsFeedDoesNotExistOnFileSystemTransitionsToCreatingNewRecentEventsFeed()
        {
            IEnumerable<Event> events = new[] {new EventBuilder().Build()};

            ITask start = new IdentifyingRecentEventsFeed(events);
            ITask end = start.Execute(new InMemoryFileSystem(), new EventBufferBuilder().Build(), new FeedBuilder(SampleLinks.Instance), (args => { }));

            Assert.IsInstanceOf(typeof (CreatingNewRecentEventsFeed), end);
            Assert.AreEqual(events, end.GetEvents());
        }

        [Test]
        public void IsNotTerminalState()
        {
            Assert.IsFalse(new IdentifyingRecentEventsFeed(new Event[]{}).IsLastTask);
        }
    }
}