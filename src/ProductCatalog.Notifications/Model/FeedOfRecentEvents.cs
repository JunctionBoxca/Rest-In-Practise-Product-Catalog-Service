using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public class FeedOfRecentEvents : IRepresentation
    {
        private readonly IRepresentation innerRepresentation;

        public FeedOfRecentEvents(IRepresentation innerRepresentation)
        {
            this.innerRepresentation = innerRepresentation;
        }

        public void UpdateContext(IResponseContext context)
        {
            context.AddHeader(CacheControl.ShortCachingPolicy);
            context.AddEntityBodyTransformationStrategyFactoryMethod(stream => new CopyEntityBody(stream));
            innerRepresentation.UpdateContext(context);
        }
    }
}