namespace ProductCatalog.Notifications.Persistence
{
    public interface IStoreId
    {
    }

    public interface IStoreId<T>
    {
        T Value { get; }
    }
}