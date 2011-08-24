using System;

namespace ProductCatalog.Notifications.Http
{
    public interface IRequestWrapper
    {
        ICondition Condition { get; }
        Uri Uri { get; }
    }
}