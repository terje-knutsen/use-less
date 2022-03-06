using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class PeriodType : Enumeration
    {
        public static PeriodType Undefined = new(1, "UNDEFINED");
        public static PeriodType Week = new(2, "WEEK");
        public static PeriodType Month = new(3, "MONTH");
        public static PeriodType Year = new(4, "YEAR");

        public PeriodType(int id, string name) : base(id, name)
        { }
        public static implicit operator int(PeriodType self)=> self.Id;
    }
}
