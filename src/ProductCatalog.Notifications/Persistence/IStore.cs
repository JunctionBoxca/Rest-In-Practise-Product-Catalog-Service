using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Persistence
{
    public interface IStore
    {
        IRepresentation GetArchiveFeed(IStoreId id);
        IRepresentation GetCurrentFeed(IStoreId id);
    }
}