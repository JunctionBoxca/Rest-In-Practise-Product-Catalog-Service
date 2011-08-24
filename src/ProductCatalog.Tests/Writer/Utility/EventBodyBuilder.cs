using System;
using ProductCatalog.Writer.Model;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class EventBodyBuilder
    {
        private string contentType = "application/vnd.restbucks+xml";
        private Uri href = new Uri("http://localhost/product-catalog/products/1");
        private object payload = new object();
        
        public EventBody Build()
        {
            return new EventBody(contentType, href, payload);
        }
    }
}