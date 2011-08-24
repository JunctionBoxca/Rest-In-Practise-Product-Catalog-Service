using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public interface IRepresentation
    {
        void UpdateContext(IResponseContext context);
    }
}