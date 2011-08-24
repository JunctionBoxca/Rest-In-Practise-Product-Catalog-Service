using System.ServiceModel.Syndication;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class EntryExtensions
    {
        public static SyndicationItem GetSyndicationItem(this Entry entry)
        {
            return PrivateField.GetValue<SyndicationItem>("item", entry);
        }
    }
}