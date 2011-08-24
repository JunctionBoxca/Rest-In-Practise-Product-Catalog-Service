using ProductCatalog.Notifications.Service;

namespace ProductCatalog.Notifications.Model
{
    public class GetFeedOfRecentEventsCommand : IRepositoryCommand
    {
        public static readonly IRepositoryCommand Instance = new GetFeedOfRecentEventsCommand();

        private GetFeedOfRecentEventsCommand()
        {
        }

        public IRepresentation Execute(IRepository repository)
        {
            return repository.GetFeedOfRecentEvents();
        }
    }
}