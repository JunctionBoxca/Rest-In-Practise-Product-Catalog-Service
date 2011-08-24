using System;

namespace ProductCatalog.Notifications.Http
{
    public class LastModified : IHeader
    {
        private readonly DateTime value;

        public LastModified(DateTime value)
        {
            this.value = value;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            response.WriteLastModified(value.ToString("R"));
        }
    }
}