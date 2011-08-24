using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public class HeaderQuery
    {
        private readonly IHeader header;

        public HeaderQuery(IHeader header)
        {
            this.header = header;
        }

        public bool Matches(IRepresentation representation)
        {
            IResponseContext context = new ResponseContext();
            representation.UpdateContext(context);
            return context.ContainsHeader(header);
        }
    }
}