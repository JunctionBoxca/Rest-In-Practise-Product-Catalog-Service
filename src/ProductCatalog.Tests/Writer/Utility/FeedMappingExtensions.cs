using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class FeedMappingExtensions
    {
        public static FileName GetFileName(this FeedMapping feedMapping)
        {
            return PrivateField.GetValue<FileName>("fileName", feedMapping);
        }

        public static Id GetId(this FeedMapping feedMapping)
        {
            return PrivateField.GetValue<Id>("id", feedMapping);
        }
    }
}