using System;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Notifications.Service
{
    public class NullFeedMappings : IFeedMappings
    {
        public static readonly IFeedMappings Instance = new NullFeedMappings();

        private NullFeedMappings()
        {
        }

        public Func<IStore, IRepresentation> CreateFeedOfRecentEventsAccessor()
        {
            throw new ServerException("Feed mappings not initialized.");
        }

        public Func<IStore, IRepresentation> CreateFeedAccessor(ResourceId resourceId)
        {
            throw new ServerException("Feed mappings not initialized.");
        }
    }
}