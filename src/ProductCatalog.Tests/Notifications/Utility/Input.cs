using System;
using ProductCatalog.Notifications.Http;

namespace ProductCatalog.Tests.Notifications.Utility
{
    public class Input : IRequestWrapper
    {
        private readonly Uri uri;

        public Input(Uri uri)
        {
            this.uri = uri;
        }

        public ICondition Condition
        {
            get { return NullCondition.Instance; }
        }

        public Uri Uri
        {
            get { return uri; }
        }
    }
}