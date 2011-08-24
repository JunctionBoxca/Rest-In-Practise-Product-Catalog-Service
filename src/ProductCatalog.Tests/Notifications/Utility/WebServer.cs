using System;
using System.Net;
using ProductCatalog.Notifications.Net;

namespace ProductCatalog.Tests.Notifications.Utility
{
    public class WebServer : IDisposable
    {
        private readonly HttpListener listener;

        public WebServer(string uriPrefix)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uriPrefix);
            listener.Start();
        }

        public void ExecuteRequestAndInvokeAssertions(Func<HttpWebResponse> request, Action<HttpListenerRequestWrapper> assertions)
        {
            HttpWebResponse response = ExecuteRequest(request, new AsyncParameters(assertions, r => r.Dispose()));
            response.Close();
        }

        public HttpWebResponse ExecuteRequestAndGenerateResponse(Func<HttpWebResponse> request, Action<HttpListenerResponseWrapper> generateResponse)
        {
            return ExecuteRequest(request, new AsyncParameters(requestWrapper => { }, generateResponse));
        }

        private HttpWebResponse ExecuteRequest(Func<HttpWebResponse> request, AsyncParameters asyncParameters)
        {
            listener.BeginGetContext(HandleRequest, asyncParameters);
            return request.Invoke();
        }

        private void HandleRequest(IAsyncResult ar)
        {
            var asyncParameters = (AsyncParameters) ar.AsyncState;

            HttpListenerContext context = listener.EndGetContext(ar);

            //Unpack request and assert
            var requestWrapper = new HttpListenerRequestWrapper(context.Request);
            asyncParameters.Assertions.Invoke(requestWrapper);

            //Generate response
            asyncParameters.GenerateResponse(new HttpListenerResponseWrapper(context.Response));

            context.Response.Close();
        }

        public void Dispose()
        {
            listener.Stop();
            listener.Close();
        }

        private class AsyncParameters
        {
            private readonly Action<HttpListenerRequestWrapper> assertions;
            private readonly Action<HttpListenerResponseWrapper> generateResponse;

            public AsyncParameters(Action<HttpListenerRequestWrapper> assertions, Action<HttpListenerResponseWrapper> generateResponse)
            {
                this.assertions = assertions;
                this.generateResponse = generateResponse;
            }

            public Action<HttpListenerRequestWrapper> Assertions
            {
                get { return assertions; }
            }

            public Action<HttpListenerResponseWrapper> GenerateResponse
            {
                get { return generateResponse; }
            }
        }
    }
}