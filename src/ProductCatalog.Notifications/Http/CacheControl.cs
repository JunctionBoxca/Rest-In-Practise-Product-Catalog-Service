namespace ProductCatalog.Notifications.Http
{
    public class CacheControl : IHeader
    {
        public static CacheControl LongCachingPolicy = new CacheControl("max-age=10000");
        public static CacheControl ShortCachingPolicy = new CacheControl("max-age=10");

        private readonly string value;

        private CacheControl(string value)
        {
            this.value = value;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            response.WriteCacheControl(value);
        }
    }
}