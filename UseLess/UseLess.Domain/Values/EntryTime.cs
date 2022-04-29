using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public class EntryTime : Value<EntryTime>
    {
        private readonly DateTime value;
        private EntryTime(DateTime value) => this.value = value;
        public bool IsEmpty => value == DateTime.MinValue;
        public DateTime Value => value;
        public static EntryTime Now => new(DateTime.Now);
        public static EntryTime From(DateTime dateTime)
            => new(dateTime);

        public override CompareResult CompareTo(EntryTime? other)
        => value.CompareTo(other?.value) switch
        {
            -1 => CompareResult.LESS,
            0 => CompareResult.EQUAL,
            1 => CompareResult.GREATER,
            _=> CompareResult.NOT_EQUAL
            
        };

        public static implicit operator DateTime(EntryTime self)=> self?.value ?? DateTime.MinValue;

        public EntryTime AddMonths(int months)
        => new EntryTime(value.AddMonths(months));

        public DateTime AddDays(int days)
        => new EntryTime(value.AddDays(days));

        public EntryTime AddHours(int hours)
        => new EntryTime(value.AddHours(hours));
        internal DateTime AddYears(int count)
        => new EntryTime(Value.AddYears(count));

        public override string ToString()
        => Value.ToString("yy.MM.dd HH:mm");

      
    }
}
