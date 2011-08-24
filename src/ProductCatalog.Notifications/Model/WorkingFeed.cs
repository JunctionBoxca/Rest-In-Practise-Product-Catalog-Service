using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public class WorkingFeed : IRepresentation
    {
        private readonly IRepresentation innerRepresentation;

        public WorkingFeed(IRepresentation innerRepresentation)
        {
            this.innerRepresentation = innerRepresentation;
        }

        public void UpdateContext(IResponseContext context)
        {
            context.AddHeader(CacheControl.ShortCachingPolicy);
            context.AddEntityBodyTransformationStrategyFactoryMethod(stream => new RewriteEntityBody(stream));
            innerRepresentation.UpdateContext(context);
        }
    }
}