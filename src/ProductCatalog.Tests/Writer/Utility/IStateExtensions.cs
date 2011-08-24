using System.Collections.Generic;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class IStateExtensions
    {
        public static RecentEventsFeed GetRecentEventsFeed(this ITask task)
        {
            return PrivateField.GetValue<RecentEventsFeed>("recentEventsFeed", task);
        }

        public static IEnumerable<Event> GetEvents(this ITask task)
        {
            return PrivateField.GetValue<IEnumerable<Event>>("events", task);
        }
    }
}