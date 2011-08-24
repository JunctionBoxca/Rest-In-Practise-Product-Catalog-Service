using System;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class ResourceIdToStoreIdConverterTests
    {
        [Test]
        public void WhenGivenResourceIdShouldConvertToStoreId()
        {
            ResourceId resourceId = new ResourceId("id");
            IStoreId storeId = ResourceIdToStoreIdConverter.Default.Convert(resourceId);

            Assert.AreEqual("id.atom", ((IStoreId<string>) storeId).Value);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof (NullReferenceException), ExpectedMessage = "convert cannot be null.")]
        public void IfFunctionIsNullShouldThrowException()
        {
            new ResourceIdToStoreIdConverter(null);
        }
    }
}