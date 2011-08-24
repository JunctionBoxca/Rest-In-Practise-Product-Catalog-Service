using NUnit.Framework;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Tests.Notifications.Persistence
{
    [TestFixture]
    public class StoreIdTests
    {
        [Test]
        public void ShouldExhibitValueTypeEquality()
        {
            StoreId<string> storeId1 = new StoreId<string>("A");
            StoreId<string> storeId2 = new StoreId<string>("A");
            StoreId<string> storeId3 = new StoreId<string>("B");

            Assert.True(storeId1.Equals(storeId2));
            Assert.False(storeId1.Equals(storeId3));
            Assert.True(storeId1.Equals(storeId1));
            Assert.False(storeId1.Equals(new object()));
            Assert.False(storeId1.Equals(null));

            Assert.True(storeId1.GetHashCode().Equals(storeId2.GetHashCode()));
            Assert.False(storeId1.GetHashCode().Equals(storeId3.GetHashCode()));
            Assert.True(storeId1.GetHashCode().Equals(storeId1.GetHashCode()));
            Assert.False(storeId1.GetHashCode().Equals(new object().GetHashCode()));
        }
    }
}