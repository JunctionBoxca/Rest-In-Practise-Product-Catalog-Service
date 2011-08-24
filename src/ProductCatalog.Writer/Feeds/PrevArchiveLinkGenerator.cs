using System.ServiceModel.Syndication;

namespace ProductCatalog.Writer.Feeds
{
    public class PrevArchiveLinkGenerator : IPrevArchiveLinkGenerator
    {
        public static readonly IPrevArchiveLinkGenerator Null = new NullPrevArchiveLinkGenerator();

        private readonly Id id;
        
        public PrevArchiveLinkGenerator(Id id)
        {
            this.id = id;
        }

        public void AddTo(SyndicationFeed feed, Links links)
        {
            feed.Links.Add(links.CreatePrevArchiveLink(id));
        }

        private class NullPrevArchiveLinkGenerator : IPrevArchiveLinkGenerator
        {
            public void AddTo(SyndicationFeed feed, Links links)
            {
                //Do nothing
            }
        }
    }
}