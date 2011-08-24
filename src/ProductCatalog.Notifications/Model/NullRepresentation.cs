using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Model
{
    public class NullRepresentation : IRepresentation
    {
        public static IRepresentation Instance = new NullRepresentation();

        private NullRepresentation()
        {
        }

        public bool IsMatch(ETag eTag)
        {
            return false;
        }

        public void UpdateContext(IResponseContext context)
        {
            //Do nothing
        }
    }
}