using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Service
{
    public interface IRepositoryCommand
    {
        IRepresentation Execute(IRepository repository);
    }
}