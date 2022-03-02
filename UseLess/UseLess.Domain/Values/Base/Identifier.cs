using UseLess.Framework;

namespace UseLess.Domain.Values.Base
{
    public abstract class Identifier<T> : Value<Identifier<T>> where T : Identifier<T>,new()
    {
        public Guid Id { get; private set; }
        public static T From(Guid guid) => new() { Id = guid};
        public static T From(string value)=> new() { Id = Guid.Parse(value) };
        public static implicit operator Guid(Identifier<T> self)=> self?.Id ?? default;
        protected override bool CompareProperties(Identifier<T> other)
        => Id == other?.Id;
        public override string ToString()
        => Id.ToString();

    }
}
