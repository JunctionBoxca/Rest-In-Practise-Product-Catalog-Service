using System;

namespace ProductCatalog.Shared
{
    public class UriConfiguration
    {
        private readonly Uri baseAddress;
        private readonly UriTemplate recentFeedTemplate;
        private readonly UriTemplate feedTemplate;
        private readonly UriTemplate entryTemplate;

        public UriConfiguration(Uri baseAddress, UriTemplate recentFeedTemplate, UriTemplate feedTemplate, UriTemplate entryTemplate)
        {
            this.baseAddress = baseAddress;
            this.recentFeedTemplate = recentFeedTemplate;
            this.feedTemplate = feedTemplate;
            this.entryTemplate = entryTemplate;
        }

        public Uri BaseAddress
        {
            get { return baseAddress; }
        }

        public UriTemplate RecentFeedTemplate
        {
            get { return recentFeedTemplate; }
        }

        public UriTemplate FeedTemplate
        {
            get { return feedTemplate; }
        }

        public UriTemplate EntryTemplate
        {
            get { return entryTemplate; }
        }
    }
}