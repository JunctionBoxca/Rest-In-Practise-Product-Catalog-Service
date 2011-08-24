namespace ProductCatalog.Notifications.Http
{
    public interface IHeader
    {
        void AddToResponse(IResponseWrapper response);
    }
}