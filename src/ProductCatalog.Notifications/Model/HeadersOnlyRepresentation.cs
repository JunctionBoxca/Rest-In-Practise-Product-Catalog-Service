using System.Collections.Generic;
using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public class HeadersOnlyRepresentation : IRepresentation
    {
        private readonly IRepresentation innerRepresentation;
        private readonly IEnumerable<IHeader> headers;

        public HeadersOnlyRepresentation(params IHeader[] headers) : this(NullRepresentation.Instance, headers)
        {
        }

        public HeadersOnlyRepresentation(IRepresentation innerRepresentation, params IHeader[] headers)
        {
            this.innerRepresentation = innerRepresentation;
            this.headers = headers;
        }

        public void UpdateContext(IResponseContext context)
        {
            foreach (IHeader header in headers)
            {
                context.AddHeader(header);
            }
            innerRepresentation.UpdateContext(context);
        }
    }
}