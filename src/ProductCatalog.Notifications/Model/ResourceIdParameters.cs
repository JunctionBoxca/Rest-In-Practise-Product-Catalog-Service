using System.Collections.Specialized;

namespace ProductCatalog.Notifications.Models
{
    public class ResourceIdParameters
    {
        private readonly NameValueCollection parameters;

        public ResourceIdParameters(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public string this[int index]
        {
            get { return parameters[index]; }
        }
    }
}