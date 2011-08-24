using System;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Notifications.Model
{
    public class FeedMappings : IFeedMappings
    {
        private readonly ResourceIdToStoreIdConverter converter;
        private readonly ResourceId workingFeedResourceId;
        private readonly IStoreId recentEventsFeedStoreId;

        public FeedMappings(ResourceIdToStoreIdConverter converter, ResourceId workingFeedResourceId, IStoreId recentEventsFeedStoreId)
        {
            this.converter = converter;
            this.workingFeedResourceId = workingFeedResourceId;
            this.recentEventsFeedStoreId = recentEventsFeedStoreId;
        }

        public Func<IStore, IRepresentation> CreateFeedOfRecentEventsAccessor()
        {
            return (store => new FeedOfRecentEvents(store.GetCurrentFeed(recentEventsFeedStoreId)));
        }

        public Func<IStore, IRepresentation> CreateFeedAccessor(ResourceId resourceId)
        {
            if (resourceId.Equals(workingFeedResourceId))
            {
                return (store => new WorkingFeed(store.GetCurrentFeed(recentEventsFeedStoreId)));
            }

            return (store => new ArchiveFeed(store.GetArchiveFeed(converter.Convert(resourceId))));
        }
    }
}