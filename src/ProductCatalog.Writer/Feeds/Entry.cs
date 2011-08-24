using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class Entry
    {
        private readonly SyndicationItem item;
        private readonly FileName fileName;

        public Entry(SyndicationItem item, FileName fileName)
        {
            this.item = item;
            this.fileName = fileName;
        }

        public void AddTo(SyndicationFeed feed)
        {
            ((IList<SyndicationItem>)feed.Items).Insert(0, item);
        }

        public void Save(IFileSystem fileSystem)
        {
            using (XmlWriter writer = fileSystem.EntryDirectory.GetXmlWriter(fileName))
            {
                item.GetAtom10Formatter().WriteTo(writer);
            }
        }
    }
}