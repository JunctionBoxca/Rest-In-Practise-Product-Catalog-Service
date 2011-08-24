using System;
using System.Runtime.Serialization;

namespace ProductCatalog.Notifications.Service
{
    public class InvalidUriException : Exception
    {
        public InvalidUriException()
        {
        }

        public InvalidUriException(string message) : base(message)
        {
        }

        public InvalidUriException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidUriException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}