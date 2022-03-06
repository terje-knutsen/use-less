using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class PeriodState : Enumeration
    {
        public static PeriodState Cyclic = new(1,"CYCLICAL");
        public static PeriodState NonCyclic = new(2, "STATIC");
        public PeriodState(int id, string name) : base(id, name)
        { }
    }
}
