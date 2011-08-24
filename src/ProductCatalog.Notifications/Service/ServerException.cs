using System;
using System.Runtime.Serialization;

namespace ProductCatalog.Notifications.Service
{
    public class ServerException : Exception
    {
        public ServerException()
        {
        }

        public ServerException(string message) : base(message)
        {
        }

        public ServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}