using UseLess.Framework;

namespace UseLess.Domain.Values.Base
{
    public abstract class PeriodTime<T> : Value<PeriodTime<T>> where T : PeriodTime<T>,new()
    {
        public DateTime Time { get; protected set; }
        public bool IsEmpty => Time == DateTime.MinValue;
        public static T From(DateTime value) => new() {Time = value };
        public override CompareResult CompareTo(PeriodTime<T>? other)
        => Time.CompareTo(other?.Time) switch
        {
            -1 => CompareResult.LESS,
            0 => CompareResult.EQUAL,
            1 => CompareResult.GREATER,
            _=> CompareResult.NOT_EQUAL
        };
        public static implicit operator DateTime(PeriodTime<T> self)=> self?.Time ?? DateTime.MinValue;
    }
}
