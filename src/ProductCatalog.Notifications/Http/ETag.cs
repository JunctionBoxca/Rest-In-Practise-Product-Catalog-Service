namespace ProductCatalog.Notifications.Http
{
    public class ETag : IHeader
    {
        private readonly string value;

        public ETag(string value)
        {
            this.value = value;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            response.WriteETag(value);
        }

        public bool Equals(ETag other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ETag)) return false;
            return Equals((ETag) obj);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }
    }
}