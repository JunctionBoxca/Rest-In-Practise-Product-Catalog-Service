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
    public class IdentifyingRecentEventsFeed : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly IEnumerable<Event> events;

        public IdentifyingRecentEventsFeed(IEnumerable<Event> events)
        {
            this.events = events;

            Log.Debug("Identifying recent events feed.");
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            FileName fileName = fileSystem.CurrentDirectory.GetLatest();
            if (fileName != null)
            {
                RecentEventsFeed recentEventsFeed = feedBuilder.LoadRecentEventsFeed(fileSystem, fileName);
                Log.DebugFormat("Found recent events feed. {0}", recentEventsFeed);
                return new UpdatingRecentEventsFeed(recentEventsFeed, events);
            }

            Log.Debug("No recent events feed found.");

            return new CreatingNewRecentEventsFeed(events);
        }
    }
}