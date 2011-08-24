using System.Linq;
using System.ServiceModel.Syndication;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class RecentEventsFeedExtensions
    {
        public static FeedMapping GetFeedMapping(this RecentEventsFeed recentEventsFeed)
        {
            return PrivateField.GetValue<FeedMapping>("mapping", recentEventsFeed);
        }

        public static SyndicationFeed GetSyndicationFeed(this RecentEventsFeed recentEventsFeed)
        {
            return PrivateField.GetValue<SyndicationFeed>("feed", recentEventsFeed);
        }

        public static int GetNumberOfEntries(this RecentEventsFeed recentEventsFeed)
        {
            return recentEventsFeed.GetSyndicationFeed().Items.Count();
        }
    }
}