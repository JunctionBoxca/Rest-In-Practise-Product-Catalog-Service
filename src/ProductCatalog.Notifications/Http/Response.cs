using System.Reflection;
using log4net;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public class Response : IResponse
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static IResponse OK(IRepresentation representation)
        {
            return new Response(StatusCode.OK, representation);
        }

        public static IResponse NotModified(ETag eTag)
        {
            return new Response(StatusCode.NotModified, new HeadersOnlyRepresentation(eTag));
        }

        public static IResponse NotFound()
        {
            return new Response(StatusCode.NotFound);
        }

        public static IResponse InternalServerError()
        {
            return new Response(StatusCode.InternalServerError);
        }

        private readonly StatusCode statusCode;
        private readonly IRepresentation representation;

        private Response(StatusCode statusCode, IRepresentation representation)
        {
            Log.DebugFormat("Returning {0}.", statusCode);

            this.statusCode = statusCode;
            this.representation = representation;
        }

        public Response(StatusCode statusCode) : this(statusCode, NullRepresentation.Instance)
        {
        }

        public void ApplyTo(IResponseWrapper responseWrapper)
        {
            ResponseContext context = new ResponseContext();
            context.AddHeader(statusCode);
            representation.UpdateContext(context);
            context.ApplyTo(responseWrapper);
        }
    }
}