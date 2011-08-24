using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class FeedBuilder
    {
        private const string ServiceName = "Product Catalog";
        private const string Title = "Restbucks products and promotions";

        private readonly Links links;

        public FeedBuilder(Links links)
        {
            this.links = links;
        }

        public RecentEventsFeed LoadRecentEventsFeed(IFileSystem fileSystem, FileName fileName)
        {
            using (XmlReader reader = fileSystem.CurrentDirectory.GetXmlReader(fileName))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                Id id = links.GetIdFromFeedUri(feed.GetViaLink().GetAbsoluteUri());
                return new RecentEventsFeed(feed, new FeedMapping(id), this);
            }
        }

        public RecentEventsFeed CreateRecentEventsFeed(FeedMapping mapping, IPrevArchiveLinkGenerator prevArchiveLinkGenerator)
        {
            SyndicationFeed feed = new SyndicationFeed
                                       {
                                           Id = new UniqueId(Guid.NewGuid()).ToString(),
                                           Title = SyndicationContent.CreatePlaintextContent(Title),
                                           Generator = ServiceName,
                                           LastUpdatedTime = DateTime.Now,
                                           Items = new List<SyndicationItem>()
                                       };

            feed.Authors.Add(new SyndicationPerson {Name = ServiceName});

            feed.Links.Add(links.CreateRecentFeedSelfLink());
            feed.Links.Add(links.CreateViaLink(mapping.Id));

            prevArchiveLinkGenerator.AddTo(feed, links);

            return new RecentEventsFeed(feed, mapping, this);
        }

        public RecentEventsFeed CreateNextRecentEventsFeed(FeedMapping mapping)
        {
            return CreateRecentEventsFeed(mapping.WithNextId(), new PrevArchiveLinkGenerator(mapping.Id));
        }

        public ArchiveFeed CreateArchiveFeed(SyndicationFeed feed, FeedMapping currentMapping, FeedMapping nextMapping)
        {
            SyndicationFeed archive = feed.Clone(true);

            archive.GetSelfLink().Uri = archive.GetViaLink().Uri;
            archive.Links.Remove(archive.GetViaLink());
            archive.Links.Add(links.CreateNextArchiveLink(nextMapping.Id));
            archive.ElementExtensions.Add(new SyndicationElementExtension("archive", "http://purl.org/syndication/history/1.0", string.Empty));

            return new ArchiveFeed(archive, currentMapping.WithArchiveFileName());
        }

        public Entry CreateEntry(Event evnt)
        {
            SyndicationItem item = new SyndicationItem
                                       {
                                           Id = string.Format("tag:restbucks.com,{0}:{1}", evnt.Timestamp.ToString("yyyy-MM-dd"), evnt.Id),
                                           Title = SyndicationContent.CreatePlaintextContent(evnt.Subject),
                                           LastUpdatedTime = evnt.Timestamp
                                       };

            item.Links.Add(links.CreateEntrySelfLink(new Id(evnt.Id)));
            item.Links.Add(links.CreateEntryRelatedLink(evnt.Body.Href));

            item.Content = new XmlSyndicationContent(evnt.Body.ContentType, evnt.Body.Payload, null as DataContractSerializer);

            return new Entry(item, new Id(evnt.Id).CreateFileName());
        }
    }
}