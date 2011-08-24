using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public class NullCondition : ICondition
    {
        public static readonly ICondition Instance = new NullCondition();

        private NullCondition()
        {
        }

        public IResponse CreateResponse(IRepresentation representation)
        {
            return Response.OK(representation);
        }
    }
}