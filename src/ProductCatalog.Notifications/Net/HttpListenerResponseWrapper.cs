using System.Net;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Net
{
    public class HttpListenerResponseWrapper : IResponseWrapper
    {
        private readonly HttpListenerResponse response;

        public HttpListenerResponseWrapper(HttpListenerResponse response)
        {
            this.response = response;
        }

        public void WriteIsChunked(bool value)
        {
            response.SendChunked = value;
        }

        public void WriteStatusCode(int value)
        {
            response.StatusCode = value;
        }

        public void WriteStatusDescription(string value)
        {
            response.StatusDescription = value;
        }

        public void WriteCacheControl(string value)
        {
            response.Headers[HttpResponseHeader.CacheControl] = value;
        }

        public void WriteContentLength(long value)
        {
            response.ContentLength64 = value;
        }

        public void WriteContentType(string value)
        {
            response.ContentType = value;
        }

        public void WriteETag(string value)
        {
            response.Headers[HttpResponseHeader.ETag] = value;
        }

        public void WriteLastModified(string value)
        {
            response.Headers[HttpResponseHeader.LastModified] = value;
        }

        public void WriteEntityBody(IEntityBodyTransformationStrategy transformationStrategy)
        {
            transformationStrategy.WriteEntityBody(response.OutputStream);
        }

        public void Dispose()
        {
            response.Close();
        }
    }
}