namespace UseLess.Framework
{
    public abstract class Value<T> : IEquatable<T>
    {
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(ReferenceEquals(this, obj)) return true;
            return obj is T other && Equals((T)other);
        }
        public bool Equals(T? other)
        => CompareProperties(other);
        protected abstract bool CompareProperties(T? other);
        public override int GetHashCode()
        {
            var hashCode = 1779266240;
            foreach (var prop in GetType().GetProperties())
                hashCode = hashCode * -1521134295 + prop.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Value<T> a, Value<T> b) => Equals(a, b);
        public static bool operator !=(Value<T> a, Value<T> b) => !Equals(a, b);
    }
}
