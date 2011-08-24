using ProductCatalog.Notifications.Service;

namespace ProductCatalog.Notifications.Model
{
    public class GetFeedCommand : IRepositoryCommand
    {
        private readonly ResourceId id;

        public GetFeedCommand(ResourceId id)
        {
            this.id = id;
        }

        public IRepresentation Execute(IRepository repository)
        {
            return repository.GetFeed(id);
        }
    }
}