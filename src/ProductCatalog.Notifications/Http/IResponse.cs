namespace ProductCatalog.Notifications.Http
{
    public interface IResponse
    {
        void ApplyTo(IResponseWrapper responseWrapper);
    }
}