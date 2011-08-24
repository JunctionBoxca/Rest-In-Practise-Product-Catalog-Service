using System;

namespace ProductCatalog.Writer.Model
{
    public class EventBody
    {
        private readonly string contentType;
        private readonly Uri href;
        private readonly object payload;

        public EventBody(string contentType, Uri href, object payload)
        {
            this.contentType = contentType;
            this.href = href;
            this.payload = payload;
        }

        public string ContentType
        {
            get { return contentType; }
        }

        public Uri Href
        {
            get { return href; }
        }

        public object Payload
        {
            get { return payload; }
        }
    }
}