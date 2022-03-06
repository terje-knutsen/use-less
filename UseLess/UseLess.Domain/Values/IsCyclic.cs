using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class IsCyclic : Value<IsCyclic>
    {
        private readonly bool value;
        private IsCyclic(bool value) => this.value = value;
        public static IsCyclic From(bool value) => new(value);
        public static implicit operator bool(IsCyclic self) => self?.value ?? false;
        public override CompareResult CompareTo(IsCyclic? other)
        => value == other?.value ? CompareResult.EQUAL : CompareResult.NOT_EQUAL;
    }
}
