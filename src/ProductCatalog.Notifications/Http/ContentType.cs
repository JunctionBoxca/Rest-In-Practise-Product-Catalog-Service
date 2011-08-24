namespace ProductCatalog.Notifications.Http
{
    public class ContentType : IHeader
    {
        public static readonly ContentType Atom = new ContentType("application/atom+xml");

        private readonly string value;

        private ContentType(string value)
        {
            this.value = value;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            response.WriteContentType(value);
        }
    }
}