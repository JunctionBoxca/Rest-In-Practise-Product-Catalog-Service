using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Tests.Shared.Utility;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class SyndicationFeeds
    {
        public static SyndicationFeed Current()
        {
            using (XmlReader reader = XmlReader.Create(ResourceStreams.CurrentFeed()))
            {
                return SyndicationFeed.Load(reader);
            }
        }
    }
}