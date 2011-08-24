using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Tests.Notifications.Utility
{
    public class Output : IResponseWrapper
    {
        public static Output For(IRepresentation representation)
        {
            ResponseContext context = new ResponseContext();
            representation.UpdateContext(context);

            return For(context);
        }

        public static Output For(IHeader header)
        {
            ResponseContext context = new ResponseContext();
            context.AddHeader(header);

            return For(context);
        }

        public static Output For(IResponse response)
        {
            Output output = new Output();
            response.ApplyTo(output);
            return output;
        }

        private Output()
        {
        }

        public bool? IsChunked { get; private set; }
        public int? StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public string CacheControl { get; private set; }
        public long? ContentLength { get; private set; }
        public string ContentType { get; private set; }
        public string ETag { get; private set; }
        public string LastModified { get; private set; }
        public string EntityBody { get; private set; }

        public void WriteIsChunked(bool value)
        {
            IsChunked = value;
        }

        public void WriteStatusCode(int value)
        {
            StatusCode = value;
        }

        public void WriteStatusDescription(string value)
        {
            StatusDescription = value;
        }

        public void WriteCacheControl(string value)
        {
            CacheControl = value;
        }

        public void WriteContentLength(long value)
        {
            ContentLength = value;
        }

        public void WriteContentType(string value)
        {
            ContentType = value;
        }

        public void WriteETag(string value)
        {
            ETag = value;
        }

        public void WriteLastModified(string value)
        {
            LastModified = value;
        }

        public void WriteEntityBody(IEntityBodyTransformationStrategy transformationStrategy)
        {
            MemoryStream destination = new MemoryStream();
            transformationStrategy.WriteEntityBody(destination);
            destination.Seek(0, SeekOrigin.Begin);

            if (destination.Length == 0)
            {
                return;
            }

            using (StreamReader sr = new StreamReader(destination))
            {
                EntityBody = sr.ReadToEnd();
            }
        }

        public void Dispose()
        {
            //Do nothing
        }

        public SyndicationFeed ToSyndicationFeed()
        {
            StringReader sr = new StringReader(EntityBody);
            return SyndicationFeed.Load(XmlReader.Create(sr));
        }
    }
}