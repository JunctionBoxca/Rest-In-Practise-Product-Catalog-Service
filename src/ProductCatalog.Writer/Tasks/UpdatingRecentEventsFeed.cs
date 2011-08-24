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
    public class UpdatingRecentEventsFeed : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly IEnumerable<Event> events;
        private readonly RecentEventsFeed recentEventsFeed;

        public UpdatingRecentEventsFeed(RecentEventsFeed recentEventsFeed, IEnumerable<Event> events)
        {
            this.recentEventsFeed = recentEventsFeed;
            this.events = events;

            Log.DebugFormat("Updating recent events feed. {0}", recentEventsFeed);
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            Queue<Event> remainingEvents = new Queue<Event>();
            
            foreach (Event evnt in events)
            {
                if (recentEventsFeed.IsFull())
                {
                    remainingEvents.Enqueue(evnt);                   
                }
                else
                {
                    recentEventsFeed.AddEvent(evnt);
                }               
            }

            if (remainingEvents.Count > 0)
            {
                return new ArchivingRecentEventsFeed(recentEventsFeed, remainingEvents);
            }

            return new RequeryingEvents(recentEventsFeed);
        }
    }
}