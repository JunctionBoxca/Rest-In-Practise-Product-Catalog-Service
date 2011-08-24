using System.Linq;
using System.ServiceModel.Syndication;

namespace ProductCatalog.Shared
{
    public static class SyndicationItemExtensions
    {
        public static SyndicationLink GetSelfLink(this SyndicationItem item)
        {
            return item.Links.FirstOrDefault(l => l.RelationshipType.Equals("self"));
        }

        public static SyndicationLink GetRelatedLink(this SyndicationItem item)
        {
            return item.Links.FirstOrDefault(l => l.RelationshipType.Equals("related"));
        }
    }
}