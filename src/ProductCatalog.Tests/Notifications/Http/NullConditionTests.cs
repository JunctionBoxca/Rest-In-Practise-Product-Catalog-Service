using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Http
{
    [TestFixture]
    public class NullConditionTests
    {
        [Test]
        public void ShouldAlwaysReturnOK()
        {
            IRepresentation representation = new HeadersOnlyRepresentation(new ETag("etag"));

            ICondition condition = NullCondition.Instance;
            IResponse response = condition.CreateResponse(representation);

            Output output = Output.For(response);

            Assert.AreEqual(200, output.StatusCode);
            Assert.AreEqual("OK", output.StatusDescription);
        }
    }
}