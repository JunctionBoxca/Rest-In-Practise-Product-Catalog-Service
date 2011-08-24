namespace ProductCatalog.Notifications.Model
{
    public interface IRepository
    {
        IRepresentation GetFeedOfRecentEvents();
        IRepresentation GetFeed(ResourceId id);
    }
}