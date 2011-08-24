using System;

namespace ProductCatalog.Notifications.Http
{
    public class SendChunked : IChunkingStrategy, IHeader
    {
        private readonly bool sendChunked;

        public SendChunked(bool sendChunked)
        {
            this.sendChunked = sendChunked;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            if (!sendChunked)
            {
                throw new InvalidOperationException("Cannot write chunked response while sendChunked is false. Use ContentLength instead.");
            }

            response.WriteIsChunked(true);
        }

        public IHeader CreateHeader(long contentLength)
        {
            if (sendChunked)
            {
                return this;
            }

            return new ContentLength(contentLength);
        }

        private class ContentLength : IHeader
        {
            private readonly long value;

            public ContentLength(long value)
            {
                this.value = value;
            }

            public void AddToResponse(IResponseWrapper response)
            {
                response.WriteIsChunked(false);
                response.WriteContentLength(value);
            }
        }
    }
}