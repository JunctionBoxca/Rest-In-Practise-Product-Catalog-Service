using System;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;
using Rhino.Mocks;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class RepositoryTests
    {
        private static string ResourceIdValue = "current";
        private static string StoreIdValue = "current.atom";

        [Test]
        public void ShouldGetFeedOfRecentEvents()
        {
            IStore store = CreateStubStore();

            Repository repository = new Repository(store, ResourceIdToStoreIdConverter.Default);
            repository.OnFeedMappingsChanged(null, new FeedMappingsChangedEventArgs(ResourceIdValue, StoreIdValue));
            IRepresentation representation = repository.GetFeedOfRecentEvents();

            Output output = Output.For(representation);

            Assert.AreEqual(StreamBackedRepresentation.CreateCurrentFeed().GetContents(), output.EntityBody);
        }

        [Test]
        public void ShouldGetAllOtherFeeds()
        {
            IStore store = CreateStubStore();

            Repository repository = new Repository(store, ResourceIdToStoreIdConverter.Default);
            repository.OnFeedMappingsChanged(null, new FeedMappingsChangedEventArgs(ResourceIdValue, StoreIdValue));
            IRepresentation representation = repository.GetFeed(new ResourceId("archive"));

            Output output = Output.For(representation);

            Assert.AreEqual(StreamBackedRepresentation.CreateArchiveFeed().GetContents(), output.EntityBody);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "store cannot be null.")]
        public void IfStoreIsNullShouldThrowException()
        {
            new Repository(null, ResourceIdToStoreIdConverter.Default);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "converter cannot be null.")]
        public void IfConverterIsNullShouldThrowException()
        {
            new Repository((IStore) new MockRepository().Stub(typeof (IStore)), null);
        }

        private IStore CreateStubStore()
        {
            MockRepository mocks = new MockRepository();
            IStore store = (IStore) mocks.Stub(typeof (IStore));

            IRepresentation currentFeed = StreamBackedRepresentation.CreateCurrentFeed();
            IRepresentation archiveFeed = StreamBackedRepresentation.CreateArchiveFeed();

            using (mocks.Record())
            {
                SetupResult.For(store.GetCurrentFeed(new StoreId<string>("current.atom"))).Return(currentFeed);
                SetupResult.For(store.GetArchiveFeed(new StoreId<string>("archive.atom"))).Return(archiveFeed);
            }

            mocks.ReplayAll();
            return store;
        }
    }
}