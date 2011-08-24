using System;
using NUnit.Framework;
using ProductCatalog.Notifications.Service;

namespace ProductCatalog.Tests.Notifications.Service
{
    [TestFixture]
    public class ServiceHostTests
    {
        [Test]
        [ExpectedException(typeof (NullReferenceException), ExpectedMessage = "service cannot be null.")]
        public void IfServiceIsNullShouldThrowException()
        {
            new ServiceHost(null);
        }
    }
}