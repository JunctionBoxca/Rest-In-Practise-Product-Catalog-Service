using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class ArchiveFeed
    {
        private readonly SyndicationFeed feed;
        private readonly FeedMapping feedMapping;

        public ArchiveFeed(SyndicationFeed feed, FeedMapping feedMapping)
        {
            this.feed = feed;
            this.feedMapping = feedMapping;
        }

        public void Save(IFileSystem fileSystem)
        {
            using (XmlWriter writer = fileSystem.ArchiveDirectory.GetXmlWriter(feedMapping.FileName))
            {
                feed.GetAtom10Formatter().WriteTo(writer);
            }
        }

        public override string ToString()
        {
            return string.Format("Archive feed. Id: [{0}]. FileName: [{1}]. Number of entries: [{2}].", feedMapping.Id, feedMapping.FileName, feed.Items.Count());
        }
    }
}