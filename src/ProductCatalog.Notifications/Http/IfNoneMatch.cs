using System.Reflection;
using log4net;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public class IfNoneMatch : ICondition
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ETag eTag;

        public IfNoneMatch(ETag eTag)
        {
            this.eTag = eTag;
        }

        public IResponse CreateResponse(IRepresentation representation)
        {
            HeaderQuery query = new HeaderQuery(eTag);

            if (query.Matches(representation))
            {
                Log.DebugFormat("If-None-Match precondition failed.");
                return Response.NotModified(eTag);
            }

            Log.DebugFormat("If-None-Match precondition OK.");
            return Response.OK(representation);
        }
    }
}