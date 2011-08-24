using System;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Notifications.Service;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;
using Rhino.Mocks;

namespace ProductCatalog.Tests.Notifications.Service
{
    [TestFixture]
    public class RoutesTests
    {
        private static readonly string AnotherFeedId = "another";
        private static readonly string NotFoundFeedId = "not-found";

        private static readonly UriConfiguration UriConfiguration = new UriConfiguration(new Uri("http://restbucks.com/product-catalog/notifications/"), new UriTemplate("/recent"),
            new UriTemplate("/?page={id}"),
            new UriTemplate("/notification/{id}"));

        [Test]
        public void WhenRequestIsForFeedOfRecentEventsShouldReturnCommandThatGetsCurrentFeed()
        {
            IRepository repository = CreateStubRepository();

            Routes routes = new Routes(UriConfiguration);
            IRepositoryCommand command = routes.CreateCommand(new Uri("http://restbucks.com/product-catalog/notifications/recent"));
            IRepresentation representation = command.Execute(repository);

            Output output = Output.For(representation);

            Assert.AreEqual(StreamBackedRepresentation.CreateCurrentFeed().GetContents(), output.EntityBody);
        }

        [Test]
        public void WhenRequestIsForAnotherFeedShouldReturnCommandThatGetsAnotherFeed()
        {
            IRepository repository = CreateStubRepository();

            Routes routes = new Routes(UriConfiguration);
            IRepositoryCommand command = routes.CreateCommand(new Uri("http://restbucks.com/product-catalog/notifications/?page=" + AnotherFeedId));
            IRepresentation representation = command.Execute(repository);

            Output output = Output.For(representation);

            Assert.AreEqual(StreamBackedRepresentation.CreateArchiveFeed().GetContents(), output.EntityBody);
        }

        [Test]
        [ExpectedException(typeof (InvalidUriException))]
        public void IfUriIsNotRecognizedThrowsException()
        {
            Routes routes = new Routes(UriConfiguration);
            routes.CreateCommand(new Uri("http://restbucks.com/product-catalog/notifications/mon/jan"));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "uriConfiguration cannot be null.")]
        public void ShouldThrowExceptionIfUriConfigurationIsNull()
        {
            new Routes(null);
        }

        private IRepository CreateStubRepository()
        {
            MockRepository mocks = new MockRepository();
            IRepository repository = (IRepository) mocks.Stub(typeof (IRepository));

            IRepresentation currentFeed = StreamBackedRepresentation.CreateCurrentFeed();
            IRepresentation anotherFeed = StreamBackedRepresentation.CreateArchiveFeed();

            using (mocks.Record())
            {
                SetupResult.For(repository.GetFeedOfRecentEvents()).Return(currentFeed);
                SetupResult.For(repository.GetFeed(new ResourceId(AnotherFeedId))).Return(anotherFeed);
                SetupResult.For(repository.GetFeed(new ResourceId(NotFoundFeedId))).Throw(new NotFoundException());
            }

            mocks.ReplayAll();

            return repository;
        }
    }
}