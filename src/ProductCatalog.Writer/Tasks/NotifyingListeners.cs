using System;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Tasks
{
    public class NotifyingListeners : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly RecentEventsFeed recentEventsFeed;

        public NotifyingListeners(RecentEventsFeed recentEventsFeed)
        {
            this.recentEventsFeed = recentEventsFeed;

            Log.Debug("Notifying listeners.");
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            notifyMappingsChanged.Invoke(recentEventsFeed.CreateFeedMappingsChangedEventArgs());
            return new Terminate();
        }
    }
}