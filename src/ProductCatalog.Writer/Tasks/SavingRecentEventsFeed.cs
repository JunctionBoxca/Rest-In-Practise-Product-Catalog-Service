using System;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Tasks
{
    public class SavingRecentEventsFeed : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly RecentEventsFeed recentEventsFeed;
        
        public SavingRecentEventsFeed(RecentEventsFeed recentEventsFeed)
        {
            this.recentEventsFeed = recentEventsFeed;

            Log.DebugFormat("Saving recent events feed. {0}", recentEventsFeed);
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            recentEventsFeed.Save(fileSystem);
            return new NotifyingListeners(recentEventsFeed);
        }
    }
}