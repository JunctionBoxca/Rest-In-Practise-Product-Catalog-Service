using System.Reflection;
using System.Threading;
using log4net;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Notifications.Service;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications.Model
{
    public class Repository : IRepository
    {
        private readonly IStore store;
        private readonly ResourceIdToStoreIdConverter converter;
        private IFeedMappings feedMappings;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Repository(IStore store, ResourceIdToStoreIdConverter converter)
        {
            Check.IsNotNull(store, "store");
            Check.IsNotNull(converter, "converter");

            this.store = store;
            this.converter = converter;

            feedMappings = NullFeedMappings.Instance;
        }

        public IRepresentation GetFeedOfRecentEvents()
        {
            var storeAccessor = feedMappings.CreateFeedOfRecentEventsAccessor();
            return storeAccessor.Invoke(store);
        }

        public IRepresentation GetFeed(ResourceId id)
        {
            var storeAccessor = feedMappings.CreateFeedAccessor(id);
            return storeAccessor.Invoke(store);
        }

        public void OnFeedMappingsChanged(object sender, FeedMappingsChangedEventArgs args)
        {
            Log.DebugFormat("FeedMappingsChanged event. ResourceId: [{0}]. StoreId: [{1}].", args.RecentEventsFeedResourceId, args.RecentEventsFeedStoreId);

            Interlocked.Exchange(ref feedMappings, new FeedMappings(converter, new ResourceId(args.RecentEventsFeedResourceId), new StoreId<string>(args.RecentEventsFeedStoreId)));
        }
    }
}