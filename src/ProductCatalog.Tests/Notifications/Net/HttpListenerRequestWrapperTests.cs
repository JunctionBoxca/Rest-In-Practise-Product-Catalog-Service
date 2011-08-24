using System.Net;
using NUnit.Framework;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Net
{
    [TestFixture]
    public class HttpListenerRequestWrapperTests
    {
        private static string RequestUri = "http://localhost:9998/product-catalog/notifications/recent";

        [Test]
        public void ShouldReturnRequestUri()
        {
            using (WebServer webServer = new WebServer("http://localhost:9998/"))
            {
                webServer.ExecuteRequestAndInvokeAssertions(
                    () => SendRequest(RequestUri, null),
                    requestWrapper =>
                        {
                            Assert.IsNotNull(requestWrapper);
                            Assert.AreEqual(RequestUri, requestWrapper.Uri.AbsoluteUri);
                        });
            }
        }

        [Test]
        public void IfETagIsEmptyShouldReturnNullCondition()
        {
            using (WebServer webServer = new WebServer("http://localhost:9998/"))
            {
                webServer.ExecuteRequestAndInvokeAssertions(
                    () => SendRequest(RequestUri, null),
                    requestWrapper =>
                        {
                            Assert.IsNotNull(requestWrapper);
                            Assert.AreEqual(NullCondition.Instance, requestWrapper.Condition);
                        });
            }
        }

        [Test]
        public void IfETagIsNotEmptyShouldReturnIfNoneMatchCondition()
        {
            string eTagValue = "xyz";
            IRepresentation representation = new HeadersOnlyRepresentation(new ETag(eTagValue));

            using (WebServer webServer = new WebServer("http://localhost:9998/"))
            {
                webServer.ExecuteRequestAndInvokeAssertions(
                    () => SendRequest(RequestUri, eTagValue),
                    requestWrapper =>
                        {
                            Assert.IsNotNull(requestWrapper);
                            Assert.IsInstanceOf(typeof (IfNoneMatch), requestWrapper.Condition);

                            Output output = Output.For(requestWrapper.Condition.CreateResponse(representation));

                            Assert.AreEqual(304, output.StatusCode);
                            Assert.AreEqual("Not Modified", output.StatusDescription);
                        });
            }
        }

        private HttpWebResponse SendRequest(string requestUri, string eTag)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
            if (!string.IsNullOrEmpty(eTag))
            {
                request.Headers[HttpRequestHeader.IfNoneMatch] = eTag;
            }

            request.Timeout = 2000;
            return (HttpWebResponse) request.GetResponse();
        }
    }
}