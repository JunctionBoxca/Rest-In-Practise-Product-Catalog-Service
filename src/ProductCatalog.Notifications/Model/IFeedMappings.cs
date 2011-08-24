using System;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Notifications.Model
{
    public interface IFeedMappings
    {
        Func<IStore, IRepresentation> CreateFeedOfRecentEventsAccessor();
        Func<IStore, IRepresentation> CreateFeedAccessor(ResourceId resourceId);
    }
}