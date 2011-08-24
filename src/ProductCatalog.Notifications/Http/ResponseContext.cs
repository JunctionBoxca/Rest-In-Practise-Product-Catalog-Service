using System;
using System.Collections.Generic;
using System.IO;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Http
{
    public class ResponseContext : IResponseContext, IResponse
    {
        private readonly IDictionary<Type, IHeader> headers;
        private Func<Stream, IEntityBodyTransformationStrategy> entityBodyTransformationStrategyFactoryMethod;
        private Func<Stream> entityBodyStreamAccessMethod;

        public ResponseContext()
        {
            headers = new Dictionary<Type, IHeader>();
            entityBodyTransformationStrategyFactoryMethod = (stream => new CopyEntityBody(stream));
            entityBodyStreamAccessMethod = (() => Stream.Null);
        }

        public void AddEntityBodyTransformationStrategyFactoryMethod(Func<Stream, IEntityBodyTransformationStrategy> factoryMethod)
        {
            entityBodyTransformationStrategyFactoryMethod = factoryMethod;
        }

        public IResponseContext AddHeader(IHeader header)
        {
            if (headers.ContainsKey(header.GetType()))
            {
                headers[header.GetType()] = header;
            }
            else
            {
                headers.Add(header.GetType(), header);
            }

            return this;
        }

        public void AddEntityBodyStreamAccessMethod(Func<Stream> accessMethod)
        {
            entityBodyStreamAccessMethod = accessMethod;
        }

        public bool ContainsHeader(IHeader header)
        {
            if (!headers.ContainsKey(header.GetType()))
            {
                return false;
            }

            return headers[header.GetType()].Equals(header);
        }

        public void ApplyTo(IResponseWrapper response)
        {
            foreach (IHeader header in headers.Values)
            {
                header.AddToResponse(response);
            }

            IEntityBodyTransformationStrategy strategy = entityBodyTransformationStrategyFactoryMethod(entityBodyStreamAccessMethod.Invoke());
            response.WriteEntityBody(strategy);
        }
    }
}