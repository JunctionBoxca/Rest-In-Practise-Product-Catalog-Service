using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public class ArchiveFeed : IRepresentation
    {
        private readonly IRepresentation innerRepresentation;

        public ArchiveFeed(IRepresentation innerRepresentation)
        {
            this.innerRepresentation = innerRepresentation;
        }

        public void UpdateContext(IResponseContext context)
        {
            context.AddHeader(CacheControl.LongCachingPolicy);
            context.AddEntityBodyTransformationStrategyFactoryMethod(stream => new CopyEntityBody(stream));
            innerRepresentation.UpdateContext(context);
        }
    }
}