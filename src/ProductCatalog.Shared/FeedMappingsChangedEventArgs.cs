using System;

namespace ProductCatalog.Shared
{
    public class FeedMappingsChangedEventArgs : EventArgs
    {
        private readonly string recentEventsFeedResourceId;
        private readonly string recentEventsFeedStoreId;

        public FeedMappingsChangedEventArgs(string recentEventsFeedResourceId, string recentEventsFeedStoreId)
        {
            this.recentEventsFeedResourceId = recentEventsFeedResourceId;
            this.recentEventsFeedStoreId = recentEventsFeedStoreId;
        }

        public string RecentEventsFeedResourceId
        {
            get { return recentEventsFeedResourceId; }
        }

        public string RecentEventsFeedStoreId
        {
            get { return recentEventsFeedStoreId; }
        }
    }
}