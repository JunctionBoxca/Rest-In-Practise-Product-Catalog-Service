using System;
using System.Net;
using System.Reflection;
using log4net;
using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Notifications.Net
{
    public class HttpListenerRequestWrapper : IRequestWrapper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HttpListenerRequest request;

        public HttpListenerRequestWrapper(HttpListenerRequest request)
        {
            this.request = request;
        }

        public ICondition Condition
        {
            get
            {
                string eTag = request.Headers["If-None-Match"];
                if (string.IsNullOrEmpty(eTag))
                {
                    return NullCondition.Instance;
                }

                Log.DebugFormat("If-None-Match header present. ETag: [{0}].", eTag);

                return new IfNoneMatch(new ETag(eTag));
            }
        }

        public Uri Uri
        {
            get { return request.Url; }
        }
    }
}