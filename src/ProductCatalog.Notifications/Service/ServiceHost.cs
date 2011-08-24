using System;
using System.Net;
using System.Reflection;
using System.Threading;
using log4net;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Net;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications.Service
{
    public class ServiceHost : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HttpListener listener;
        private readonly NotificationsService service;

        public ServiceHost(NotificationsService service)
        {
            Check.IsNotNull(service, "service");

            this.service = service;
            listener = new HttpListener();
            listener.Prefixes.Add(service.BaseAddress.AbsoluteUri);
        }

        public void StartHost()
        {
            Log.Debug("Starting host...");

            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 listener.Start();

                                                 foreach (string prefix in listener.Prefixes)
                                                 {
                                                     Log.DebugFormat("Base uri: [{0}].", prefix);
                                                 }

                                                 while (listener.IsListening)
                                                 {
                                                     try
                                                     {
                                                         HttpListenerContext context = listener.GetContext();
                                                         ThreadPool.QueueUserWorkItem(x => HandleRequest(context));
                                                     }
                                                     catch (HttpListenerException)
                                                     {
                                                         Log.Warn("Shutting down...");
                                                     }
                                                 }
                                             });
        }

        private void HandleRequest(HttpListenerContext context)
        {
            Log.DebugFormat("{0} {1}", context.Request.HttpMethod, context.Request.RawUrl);

            IResponse response = service.GetResponse(new HttpListenerRequestWrapper(context.Request));
            using (IResponseWrapper wrapper = new HttpListenerResponseWrapper(context.Response))
            {
                response.ApplyTo(wrapper);
            }
        }

        public void Dispose()
        {
            Log.Debug("Disposing ServiceHost...");
            if (listener != null)
            {
                listener.Close();
            }
        }
    }
}