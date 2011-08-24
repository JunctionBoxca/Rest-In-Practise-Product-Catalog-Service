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
    public class CreatingNewRecentEventsFeed : ITask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly IEnumerable<Event> events;

        public CreatingNewRecentEventsFeed(IEnumerable<Event> events)
        {
            this.events = events;

            Log.Debug("Creating new recent events feed.");
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            RecentEventsFeed feed = feedBuilder.CreateRecentEventsFeed(new FeedMapping(Id.InitialValue), PrevArchiveLinkGenerator.Null);
            return new UpdatingRecentEventsFeed(feed, events);
        }
    }
}