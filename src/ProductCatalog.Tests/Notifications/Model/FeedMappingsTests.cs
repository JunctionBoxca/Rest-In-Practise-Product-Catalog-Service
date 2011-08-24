using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;
using ProductCatalog.Tests.Shared.Utility;
using Rhino.Mocks;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class FeedMappingsTests
    {
        private const string CurrentFeedResourceId = "current-resource-id";
        private const string CurrentFeedStoreId = "current-store-id";

        private const string ArchiveFeedResourceId = "archive-resource-id";
        private const string ArchiveFeedStoreId = "archive-store-id";

        private static readonly ResourceIdToStoreIdConverter IdConverter = new ResourceIdToStoreIdConverter(p => new StoreId<string>(p[0].Replace("-resource-", "-store-")));

        [Test]
        public void ShouldReturnAccessorForFeedOfRecentEvents()
        {
            IStore store = CreateStubStore();

            IFeedMappings feedMappings = new FeedMappings(IdConverter, new ResourceId(CurrentFeedResourceId), new StoreId<string>(CurrentFeedStoreId));
            Func<IStore, IRepresentation> accessor = feedMappings.CreateFeedOfRecentEventsAccessor();
            IRepresentation representation = accessor.Invoke(store);

            Output output = Output.For(representation);

            Assert.AreEqual("max-age=10", output.CacheControl);
            Assert.AreEqual(StreamBackedRepresentation.CreateCurrentFeed().GetContents(), output.EntityBody);
        }

        [Test]
        public void WhenPresentedWithResouceIdForCurrentFeedReturnsAccessorThatGetsCurrentFeedFromStore()
        {
            IStore store = CreateStubStore();

            IFeedMappings feedMappings = new FeedMappings(IdConverter, new ResourceId(CurrentFeedResourceId), new StoreId<string>(CurrentFeedStoreId));
            Func<IStore, IRepresentation> accessor = feedMappings.CreateFeedAccessor(new ResourceId(CurrentFeedResourceId));
            IRepresentation result = accessor.Invoke(store);

            Output output = Output.For(result);

            Assert.AreEqual("max-age=10", output.CacheControl);

            SyndicationFeed recent = SyndicationFeed.Load(XmlReader.Create(ResourceStreams.CurrentFeed()));
            SyndicationFeed current = SyndicationFeed.Load(XmlReader.Create(new StringReader(output.EntityBody)));

            Assert.IsNull(current.GetViaLink());
            Assert.AreEqual(recent.GetViaLink().GetAbsoluteUri(), current.GetSelfLink().GetAbsoluteUri());
        }

        [Test]
        public void WhenPresentedWithResouceIdForNonCurrentFeedReturnsAccessorThatGetsArchiveFeedFromStore()
        {
            IStore store = CreateStubStore();

            IFeedMappings feedMappings = new FeedMappings(IdConverter, new ResourceId(CurrentFeedResourceId), new StoreId<string>(CurrentFeedStoreId));
            Func<IStore, IRepresentation> accessor = feedMappings.CreateFeedAccessor(new ResourceId(ArchiveFeedResourceId));
            IRepresentation representation = accessor.Invoke(store);

            Output output = Output.For(representation);

            Assert.AreEqual("max-age=10000", output.CacheControl);
            Assert.AreEqual(StreamBackedRepresentation.CreateArchiveFeed().GetContents(), output.EntityBody);
        }

        private IStore CreateStubStore()
        {
            MockRepository mocks = new MockRepository();
            IStore store = (IStore) mocks.Stub(typeof (IStore));

            IRepresentation currentFeed = StreamBackedRepresentation.CreateCurrentFeed();
            IRepresentation anotherFeed = StreamBackedRepresentation.CreateArchiveFeed();

            using (mocks.Record())
            {
                SetupResult.For(store.GetCurrentFeed(new StoreId<string>(CurrentFeedStoreId))).Return(currentFeed);
                SetupResult.For(store.GetArchiveFeed(new StoreId<string>(ArchiveFeedStoreId))).Return(anotherFeed);
            }

            mocks.ReplayAll();
            return store;
        }
    }
}