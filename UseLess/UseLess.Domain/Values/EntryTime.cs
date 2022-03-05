using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class EntryTime : Value<EntryTime>
    {
        private readonly DateTime value;
        private EntryTime(DateTime value) => this.value = value;
        public bool IsEmpty => value == DateTime.MinValue;
  
        public static EntryTime From(DateTime dateTime)
            => new(dateTime);

        public override CompareResult CompareTo(EntryTime? other)
        {
            if (other is null) return CompareResult.GREATER;
            if(value == other.value) return CompareResult.EQUAL;
            return value < other.value ? CompareResult.LESS : CompareResult.GREATER;
        }

        public static implicit operator DateTime(EntryTime self)=> self?.value ?? DateTime.MinValue;

    }
}
