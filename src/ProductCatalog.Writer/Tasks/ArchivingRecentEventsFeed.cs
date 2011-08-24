using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Tasks
{
    public class ArchivingRecentEventsFeed : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly RecentEventsFeed recentEventsFeed;
        private readonly IEnumerable<Event> events;
        
        public ArchivingRecentEventsFeed(RecentEventsFeed recentEventsFeed, IEnumerable<Event> events)
        {
            this.recentEventsFeed = recentEventsFeed;
            this.events = events;
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            RecentEventsFeed nextFeed = recentEventsFeed.CreateNextFeed();
            ArchiveFeed archiveFeed = recentEventsFeed.CreateArchiveFeed(nextFeed);

            Log.DebugFormat("Archiving recent events feed. {0}", archiveFeed);

            archiveFeed.Save(fileSystem);

            return new UpdatingRecentEventsFeed(nextFeed, events);
        }
    }
}