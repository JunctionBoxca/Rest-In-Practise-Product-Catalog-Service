using System.Linq;
using System.ServiceModel.Syndication;

namespace ProductCatalog.Shared
{
    public static class SyndicationFeedExtensions
    {
        public static SyndicationLink GetSelfLink(this SyndicationFeed feed)
        {
            return feed.Links.FirstOrDefault(l => l.RelationshipType.Equals("self"));
        }

        public static SyndicationLink GetViaLink(this SyndicationFeed feed)
        {
            return feed.Links.FirstOrDefault(l => l.RelationshipType.Equals("via"));
        }

        public static SyndicationLink GetPrevArchiveLink(this SyndicationFeed feed)
        {
            return feed.Links.FirstOrDefault(l => l.RelationshipType.Equals("prev-archive"));
        }

        public static SyndicationLink GetNextArchiveLink(this SyndicationFeed feed)
        {
            return feed.Links.FirstOrDefault(l => l.RelationshipType.Equals("next-archive"));
        }
    }
}