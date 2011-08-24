using ProductCatalog.Notifications.Models;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Notifications.Model
{
    public interface IAcceptResourceIdParameters
    {
        IStoreId Accept(ResourceIdParameters parameters);
    }
}