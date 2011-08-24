using System;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class SampleLinks
    {
        public static readonly Links Instance =
            new Links(new UriConfiguration(new Uri("http://restbucks.com/product-catalog/notifications/"), new UriTemplate("/recent"),
                new UriTemplate("/?page={id}"),
                new UriTemplate("/notification/{id}")));
    }
}