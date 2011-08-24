using System;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public interface IResponseWrapper : IDisposable
    {
        void WriteIsChunked(bool value);
        void WriteStatusCode(int value);
        void WriteStatusDescription(string value);
        void WriteCacheControl(string value);
        void WriteContentLength(long value);
        void WriteContentType(string value);
        void WriteETag(string value);
        void WriteLastModified(string value);
        void WriteEntityBody(IEntityBodyTransformationStrategy transformationStrategy);
    }
}