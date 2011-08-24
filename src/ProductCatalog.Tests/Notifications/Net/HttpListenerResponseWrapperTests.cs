using System;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Net;
using ProductCatalog.Tests.Notifications.Utility;

namespace ProductCatalog.Tests.Notifications.Net
{
    [TestFixture]
    public class HttpListenerResponseWrapperTests
    {
        private static string RequestUri = "http://localhost:9998/product-catalog/notifications/recent";

        [Test]
        public void ShouldSetTransferEncodingHeader()
        {
            ExecuteTest(
                responseWrapper => responseWrapper.WriteIsChunked(true),
                response => Assert.AreEqual("chunked", response.Headers[HttpResponseHeader.TransferEncoding]));
        }

        [Test]
        public void ShouldSetStatusCodeAndDescription()
        {
            ExecuteTest(
                responseWrapper =>
                    {
                        responseWrapper.WriteStatusCode(201);
                        responseWrapper.WriteStatusDescription("Created");
                    },
                response =>
                    {
                        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                        Assert.AreEqual("Created", response.StatusDescription);
                    });
        }

        [Test]
        public void ShouldSetCacheControlHeader()
        {
            ExecuteTest(
                responseWrapper => responseWrapper.WriteCacheControl("max-age=123"),
                response => Assert.AreEqual("max-age=123", response.Headers[HttpResponseHeader.CacheControl]));
        }

        [Test]
        public void ShouldSetContentLengthHeader()
        {
            var entityBody = new CopyEntityBody(new MemoryStream(new byte[4]));

            ExecuteTest(
                responseWrapper =>
                    {
                        responseWrapper.WriteContentLength(4);
                        responseWrapper.WriteEntityBody(entityBody);
                    },
                response => Assert.AreEqual(4, response.ContentLength));
        }

        [Test]
        public void ShouldNotSetContentLengthHeaderWhenChunked()
        {
            var entityBody = new CopyEntityBody(new MemoryStream(new byte[4]));

            ExecuteTest(
                responseWrapper =>
                    {
                        responseWrapper.WriteContentLength(4);
                        responseWrapper.WriteIsChunked(true);
                        responseWrapper.WriteEntityBody(entityBody);
                    },
                response => Assert.IsNull(response.Headers[HttpResponseHeader.ContentLength]));
        }

        [Test]
        public void ShouldSetContentTypeHeader()
        {
            ExecuteTest(
                responseWrapper => responseWrapper.WriteContentType("application/atom+xml"),
                response => Assert.AreEqual("application/atom+xml", response.Headers[HttpResponseHeader.ContentType]));
        }

        [Test]
        public void ShouldSetETagHeader()
        {
            ExecuteTest(
                responseWrapper => responseWrapper.WriteETag("xyz"),
                response => Assert.AreEqual("xyz", response.Headers[HttpResponseHeader.ETag]));
        }

        [Test]
        public void ShouldWriteLastModifiedHeader()
        {
            string lastModified = DateTime.Now.ToString("R");

            ExecuteTest(
                responseWrapper => responseWrapper.WriteLastModified(lastModified),
                response => Assert.AreEqual(lastModified, response.Headers[HttpResponseHeader.LastModified]));
        }

        [Test]
        public void ShouldWriteEntityBody()
        {
            string content = "some content";
            var entityBody = new CopyEntityBody(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            ExecuteTest(
                responseWrapper => responseWrapper.WriteEntityBody(entityBody),
                response =>
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string foundContent = reader.ReadToEnd();
                            Assert.AreEqual(content, foundContent);
                        }
                    });
        }

        private void ExecuteTest(Action<HttpListenerResponseWrapper> generateResponse, Action<HttpWebResponse> assertions)
        {
            using (WebServer webServer = new WebServer("http://localhost:9998/"))
            {
                HttpWebResponse response = webServer.ExecuteRequestAndGenerateResponse(
                    () => SendRequest(RequestUri), generateResponse);

                assertions.Invoke(response);
                response.Close();
            }
        }

        private HttpWebResponse SendRequest(string requestUri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);

            request.Timeout = 2000;
            return (HttpWebResponse) request.GetResponse();
        }
    }
}