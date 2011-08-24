using System.Linq;
using System.ServiceModel.Syndication;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class ArchiveFeedExtensions
    {
        public static SyndicationFeed GetSyndicationFeed(this ArchiveFeed archiveFeed)
        {
            return PrivateField.GetValue<SyndicationFeed>("feed", archiveFeed);
        }

        public static int GetNumberOfEntries(this ArchiveFeed archiveFeed)
        {
            return archiveFeed.GetSyndicationFeed().Items.Count();
        }
    }
}