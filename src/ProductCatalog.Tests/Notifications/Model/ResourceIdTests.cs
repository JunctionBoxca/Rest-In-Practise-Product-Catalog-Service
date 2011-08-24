using System.Collections.Specialized;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Tests.Notifications.Model
{
    [TestFixture]
    public class ResourceIdTests
    {
        [Test]
        public void ShouldExhibitValueTypeEquality()
        {
            ResourceId resourceId1 = new ResourceId("A");
            ResourceId resourceId2 = new ResourceId("A");
            ResourceId resourceId3 = new ResourceId("B");

            Assert.True(resourceId1.Equals(resourceId2));
            Assert.False(resourceId1.Equals(resourceId3));
            Assert.True(resourceId1.Equals(resourceId1));
            Assert.False(resourceId1.Equals(new object()));
            Assert.False(resourceId1.Equals(null));

            Assert.True(resourceId1.GetHashCode().Equals(resourceId2.GetHashCode()));
            Assert.False(resourceId1.GetHashCode().Equals(resourceId3.GetHashCode()));
            Assert.True(resourceId1.GetHashCode().Equals(resourceId1.GetHashCode()));
            Assert.False(resourceId1.GetHashCode().Equals(new object().GetHashCode()));
        }

        [Test]
        public void ComplexValue()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("key1", "A");
            parameters.Add("key1", "B");
            parameters.Add("key2", "C");
            ResourceId resourceId = new ResourceId(parameters);
        }
    }
}