using System;
using System.ServiceModel.Syndication;
using ProductCatalog.Shared;

namespace ProductCatalog.Writer.Feeds
{
    public class Links
    {
        private readonly UriConfiguration config;

        public Links(UriConfiguration config)
        {
            this.config = config;
        }

        public SyndicationLink CreateRecentFeedSelfLink()
        {
            return SyndicationLink.CreateSelfLink(config.RecentFeedTemplate.BindByPosition(config.BaseAddress));
        }

        public SyndicationLink CreateViaLink(Id id)
        {
            return new SyndicationLink {RelationshipType = "via", Uri = id.CreateUri(config.BaseAddress, config.FeedTemplate)};
        }

        public SyndicationLink CreatePrevArchiveLink(Id id)
        {
            return new SyndicationLink {RelationshipType = "prev-archive", Uri = id.CreateUri(config.BaseAddress, config.FeedTemplate)};
        }

        public SyndicationLink CreateNextArchiveLink(Id id)
        {
            return new SyndicationLink { RelationshipType = "next-archive", Uri = id.CreateUri(config.BaseAddress, config.FeedTemplate) };
        }

        public SyndicationLink CreateEntrySelfLink(Id id)
        {
            return SyndicationLink.CreateSelfLink(id.CreateUri(config.BaseAddress, config.EntryTemplate));
        }

        public SyndicationLink CreateEntryRelatedLink(Uri uri)
        {
            return new SyndicationLink { RelationshipType = "related", Uri = uri };
        }

        public Uri GenerateRecentFeedUri()
        {
            return config.RecentFeedTemplate.BindByPosition(config.BaseAddress);
        }

        public Uri GenerateFeedUri(Id id)
        {
            return id.CreateUri(config.BaseAddress, config.FeedTemplate);
        }

        public Uri GenerateEntryUri(Id id)
        {
            return id.CreateUri(config.BaseAddress, config.EntryTemplate);
        }

        public Id GetIdFromFeedUri(Uri uri)
        {
            UriTemplateMatch match = config.FeedTemplate.Match(config.BaseAddress, uri);
            return new Id(int.Parse(match.BoundVariables[0]));
        }
    }
}