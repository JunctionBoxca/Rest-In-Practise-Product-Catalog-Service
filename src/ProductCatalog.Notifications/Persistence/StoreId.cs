namespace ProductCatalog.Notifications.Persistence
{
    public class StoreId<T> : IStoreId, IStoreId<T>
    {
        private readonly T value;

        public StoreId(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
        }

        public bool Equals(StoreId<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (StoreId<T>)) return false;
            return Equals((StoreId<T>) obj);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }
    }
}