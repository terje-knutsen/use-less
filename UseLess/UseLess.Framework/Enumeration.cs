using System.Reflection;

namespace UseLess.Framework
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        protected Enumeration(int id, string name) 
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        => Name;
        public static IEnumerable<T> GetAll<T>() where T : Enumeration 
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }
        public static T FromString<T>(string name) where T : Enumeration
            => GetAll<T>().ToDictionary(key => key.Name, value => value)[name];
        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration other) return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(other.Id);
            return typeMatches && valueMatches;
        }
        public int CompareTo(object? obj)
        {
            var o = obj as Enumeration;
            return Id.CompareTo(o?.Id);
        }
        public override int GetHashCode()
        => (Name, Id).GetHashCode();
    }
}
