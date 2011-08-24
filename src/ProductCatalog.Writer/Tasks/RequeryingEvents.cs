using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Tasks
{
    public class RequeryingEvents : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly RecentEventsFeed recentEventsFeed;

        public RequeryingEvents(RecentEventsFeed recentEventsFeed)
        {
            this.recentEventsFeed = recentEventsFeed;
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            IEnumerable<Event> events = buffer.Take(QueryingEvents.BatchSize);

            Log.DebugFormat("Requerying events. Number of events found: [{0}].", events.Count());

            if (events.Count().Equals(0))
            {
                return new SavingRecentEventsFeed(recentEventsFeed);
            }

            return new UpdatingRecentEventsFeed(recentEventsFeed, events);
        }
    }
}