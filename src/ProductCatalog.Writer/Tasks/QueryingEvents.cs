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
    public class QueryingEvents : ITask
    {
        public static int BatchSize = 10;
        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public QueryingEvents()
        {
            Log.Debug("Started feed writing.");
        }

        public bool IsLastTask
        {
            get { return false; }
        }

        public ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged)
        {
            IEnumerable<Event> events = buffer.Take(BatchSize);

            Log.DebugFormat("Querying events. Number of events found: [{0}].", events.Count());

            if (events.Count().Equals(0))
            {
                return new Terminate();
            }

            return new IdentifyingRecentEventsFeed(events);
        }
    }
}