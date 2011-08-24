using System;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class Id
    {
        public static readonly Id InitialValue = new Id(1);
        
        private readonly int value;

        public Id(int value)
        {
            this.value = value;
        }

        public Id Increment()
        {
            return new Id(value + 1);
        }

        public Id Clone()
        {
            return new Id(value);
        }

        public FileName CreateFileName()
        {
            return new FileName(value.ToString());
        }

        public Uri CreateUri(Uri baseAddress, UriTemplate uriTemplate)
        {
            return uriTemplate.BindByPosition(baseAddress, value.ToString());
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public bool Equals(Id other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.value == value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Id)) return false;
            return Equals((Id) obj);
        }

        public override int GetHashCode()
        {
            return value;
        }
    }
}