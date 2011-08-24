using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public interface ICondition
    {
        IResponse CreateResponse(IRepresentation representation);
    }
}