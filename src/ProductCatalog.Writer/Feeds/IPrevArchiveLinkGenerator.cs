using System.ServiceModel.Syndication;

namespace ProductCatalog.Writer.Feeds
{
    public interface IPrevArchiveLinkGenerator
    {
        void AddTo(SyndicationFeed feed, Links links);
    }
}