using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class RecentEventsFeedBuilder
    {
        public static RecentEventsFeed FullCurrentFeed()
        {
            return new RecentEventsFeedBuilder().WithNumberOfEntries(RecentEventsFeed.Quota).Build();
        }

        public static RecentEventsFeed EmptyCurrentFeed()
        {
            return new RecentEventsFeedBuilder().WithNumberOfEntries(0).Build();
        }

        public static RecentEventsFeed WithoutPreviousFeed()
        {
            return new RecentEventsFeedBuilder().Build();
        }

        private FeedMapping feedMapping = new FeedMapping(Id.InitialValue);
        private IPrevArchiveLinkGenerator prevArchiveLinkGenerator = PrevArchiveLinkGenerator.Null;
        private Links links = SampleLinks.Instance;
        private int numberOfEntries;

        public RecentEventsFeedBuilder WithId(int id)
        {
            feedMapping = new FeedMapping(new Id(id));
            return this;
        }

        public RecentEventsFeedBuilder WithPreviousId(int id)
        {
            prevArchiveLinkGenerator = new PrevArchiveLinkGenerator(new Id(id));
            return this;
        }

        public RecentEventsFeedBuilder WithUris(Links links)
        {
            this.links = links;
            return this;
        }

        public RecentEventsFeedBuilder WithNumberOfEntries(int numberOfEntries)
        {
            this.numberOfEntries = numberOfEntries;
            return this;
        }

        public RecentEventsFeed Build()
        {
            RecentEventsFeed recentEventsFeed = new FeedBuilder(links).CreateRecentEventsFeed(feedMapping, prevArchiveLinkGenerator);
            for (int i = 0; i < numberOfEntries; i++)
            {
                recentEventsFeed.AddEvent(new EventBuilder().Build());
            }
            return recentEventsFeed;
        }
    }
}