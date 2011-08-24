using System;
using System.Collections.Specialized;
using ProductCatalog.Notifications.Models;
using ProductCatalog.Notifications.Persistence;

namespace ProductCatalog.Notifications.Model
{
    public class ResourceId
    {
        private readonly NameValueCollection parameters;
        private readonly string internalValue;

        public ResourceId(string value)
        {
            internalValue = value;
            parameters = new NameValueCollection();
            parameters.Add("__DEFAULT", value);
        }

        public ResourceId(NameValueCollection parameters)
        {
            this.parameters = parameters;

            string[] values = new string[parameters.Keys.Count];
            parameters.CopyTo(values, 0);
            internalValue = string.Join(Environment.NewLine, values);
        }

        public IStoreId ToStoreId(ResourceIdToStoreIdConverter converter)
        {
            return ((IAcceptResourceIdParameters) converter).Accept(new ResourceIdParameters(parameters));
        }

        public bool Equals(ResourceId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.internalValue, internalValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ResourceId)) return false;
            return Equals((ResourceId) obj);
        }

        public override int GetHashCode()
        {
            return (internalValue != null ? internalValue.GetHashCode() : 0);
        }
    }
}