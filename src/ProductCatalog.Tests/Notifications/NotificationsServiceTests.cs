using System;
using NUnit.Framework;
using ProductCatalog.Notifications;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Notifications.Service;
using ProductCatalog.Shared;
using ProductCatalog.Tests.Notifications.Utility;
using Rhino.Mocks;

namespace ProductCatalog.Tests.Notifications
{
    [TestFixture]
    public class NotificationsServiceTests
    {
        private static readonly string AnotherFeedId = "another";
        private static readonly string NotFoundFeedId = "not-found";

        private static readonly UriConfiguration UriConfiguration = new UriConfiguration(new Uri("http://restbucks.com/product-catalog/notifications/"), new UriTemplate("/recent"),
            new UriTemplate("/?page={id}"),
            new UriTemplate("/notification/{id}"));

        [Test]
        public void WhenRequestIsForRecentFeedShouldReturnCurrentFeed()
        {
            IRepository repository = CreateStubRepository();

            NotificationsService service = CreateService(repository);
            IResponse response = service.GetResponse(new Input(new Uri("http://restbucks.com/product-catalog/notifications/recent")));

            Output output = Output.For(response);

            Assert.AreEqual(200, output.StatusCode);
            Assert.AreEqual("OK", output.StatusDescription);
            Assert.AreEqual(StreamBackedRepresentation.CreateCurrentFeed().GetContents(), output.EntityBody);
        }

        [Test]
        public void WhenRequestIsForAnyOtherFeedShouldReturnCorrespondingFeed()
        {
            IRepository repository = CreateStubRepository();

            NotificationsService service = CreateService(repository);
            IResponse response = service.GetResponse(new Input(new Uri("http://restbucks.com/product-catalog/notifications/?page=" + AnotherFeedId)));

            Output output = Output.For(response);

            Assert.AreEqual(200, output.StatusCode);
            Assert.AreEqual("OK", output.StatusDescription);
            Assert.AreEqual(StreamBackedRepresentation.CreateArchiveFeed().GetContents(), output.EntityBody);
        }

        [Test]
        public void WhenFeedDoesNotExistShouldReturnNotFound()
        {
            IRepository repository = CreateStubRepository();

            NotificationsService service = CreateService(repository);
            IResponse response = service.GetResponse(new Input(new Uri("http://restbucks.com/product-catalog/notifications/?page=" + NotFoundFeedId)));

            Output output = Output.For(response);

            Assert.AreEqual(404, output.StatusCode);
            Assert.AreEqual("Not Found", output.StatusDescription);
        }

        [Test]
        public void WhenServerEncountersErrorShouldReturnInternalServerError()
        {
            MockRepository mocks = new MockRepository();
            IRepository repository = (IRepository) mocks.Stub(typeof (IRepository));

            using (mocks.Record())
            {
                SetupResult.For(repository.GetFeedOfRecentEvents()).Throw(new ServerException());
                SetupResult.For(repository.GetFeed(null)).IgnoreArguments().Throw(new ServerException());
            }

            mocks.ReplayAll();

            NotificationsService service = CreateService(repository);
            IResponse response = service.GetResponse(new Input(new Uri("http://restbucks.com/product-catalog/notifications/recent")));

            Output output = Output.For(response);

            Assert.AreEqual(500, output.StatusCode);
            Assert.AreEqual("Internal Server Error", output.StatusDescription);
        }

        [Test]
        public void WhenUriIsNotRecognizedShouldReturnNotFound()
        {
            NotificationsService service = CreateService((IRepository) new MockRepository().CreateMock(typeof (IRepository)));
            IResponse response = service.GetResponse(new Input(new Uri("http://restbucks.com/not/recognized")));

            Output output = Output.For(response);

            Assert.AreEqual(404, output.StatusCode);
            Assert.AreEqual("Not Found", output.StatusDescription);
        }

        [Test]
        [ExpectedException(typeof (NullReferenceException), ExpectedMessage = "routes cannot be null.")]
        public void IfRoutesIsNullThrowsException()
        {
            new NotificationsService(null, (IRepository) new MockRepository().CreateMock(typeof (IRepository)));
        }

        [Test]
        [ExpectedException(typeof (NullReferenceException), ExpectedMessage = "repository cannot be null.")]
        public void IfRepositoryIsNullThrowsException()
        {
            Routes routes = new Routes(UriConfiguration);
            new NotificationsService(routes, null);
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

        private NotificationsService CreateService(IRepository repository)
        {
            Routes routes = new Routes(UriConfiguration);
            return new NotificationsService(routes, repository);
        }
    }
}