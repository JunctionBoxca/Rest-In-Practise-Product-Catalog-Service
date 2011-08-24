using System;
using System.IO;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public interface IResponseContext
    {
        void AddEntityBodyTransformationStrategyFactoryMethod(Func<Stream, IEntityBodyTransformationStrategy> factoryMethod);
        IResponseContext AddHeader(IHeader header);
        void AddEntityBodyStreamAccessMethod(Func<Stream> accessMethod);
        bool ContainsHeader(IHeader header);
    }
}