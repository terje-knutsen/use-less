using UseLess.Domain.Values.Base;

namespace UseLess.Domain.Values
{
    public sealed class StartTime : PeriodTime<StartTime>
    {
        internal StartTime AddWeek => new() { Time = Time.AddDays(7) };
        internal StartTime AddMonth => new() { Time = Time.AddMonths(1) };
        internal StartTime AddYear => new() { Time = Time.AddYears(1) };
        public static implicit operator DateTime(StartTime self)=> self.Time;
        public override string ToString()
        => Time.ToString("yy.MM.dd HH:mm");
    }
}
