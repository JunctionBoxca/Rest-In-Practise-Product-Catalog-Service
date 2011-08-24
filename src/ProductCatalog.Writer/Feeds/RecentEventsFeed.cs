using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class RecentEventsFeed
    {
        public static int Quota = 10;

        private readonly SyndicationFeed feed;
        private readonly FeedMapping mapping;
        private readonly FeedBuilder builder;

        public RecentEventsFeed(SyndicationFeed feed, FeedMapping mapping, FeedBuilder builder)
        {
            this.feed = feed;
            this.mapping = mapping;
            this.builder = builder;
        }

        public bool IsFull()
        {
            return feed.Items.Count() >= Quota;
        }

        public void AddEvent(Event evnt)
        {
            Entry entry = builder.CreateEntry(evnt);
            entry.AddTo(feed);
            feed.LastUpdatedTime = DateTime.Now;
        }

        public RecentEventsFeed CreateNextFeed()
        {
            return builder.CreateNextRecentEventsFeed(mapping);
        }

        public ArchiveFeed CreateArchiveFeed(RecentEventsFeed nextFeed)
        {
            return builder.CreateArchiveFeed(feed, mapping, nextFeed.mapping);
        }

        public FeedMappingsChangedEventArgs CreateFeedMappingsChangedEventArgs()
        {
            return mapping.CreateFeedMappingsChangedEventArgs();
        }

        public void Save(IFileSystem fileSystem)
        {
            using (XmlWriter writer = fileSystem.CurrentDirectory.GetXmlWriter(mapping.FileName))
            {
                feed.GetAtom10Formatter().WriteTo(writer);
            }
        }

        public override string ToString()
        {
            return string.Format("Current feed. Id: [{0}]. FileName: [{1}]. Number of entries: [{2}].", mapping.Id, mapping.FileName, feed.Items.Count());
        }
    }
}