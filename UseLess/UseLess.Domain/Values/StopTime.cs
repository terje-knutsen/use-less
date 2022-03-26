using UseLess.Domain.Enumerations;
using UseLess.Domain.Values.Base;
using UseLess.Messages;

namespace UseLess.Domain.Values
{
    public sealed class StopTime : PeriodTime<StopTime>
    {
        public static StopTime OneMonthFrom(DateTime value)
            => new() { Time = value.AddMonths(1) };
        public static StopTime Empty => new() { Time = DateTime.MinValue };
        public static StopTime From(StartTime startTime, PeriodType type) 
        {
            switch (type.Name) 
            {
                case "WEEK":
                    return From(startTime.AddWeek);
                case "MONTH":
                    return From(startTime.AddMonth);
                case "YEAR":
                    return From(startTime.AddYear);
                default:
                    throw Exceptions.PeriodException.WithMessage($"Period type {type.Name} cannot be used when set stop time from start time ");
            }
        }

        internal bool IsBefore(StartTime start)
        => Time <= start.Time;
    }
}
