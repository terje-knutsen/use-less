using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class IncomeType : Enumeration
    {
        public static IncomeType Salary = new(1,"SALARY");
        public static IncomeType Bonus = new(2, "BONUS");
        public static IncomeType Perks = new(3, "PERKS");
        public static IncomeType Gambling = new(4, "GAMBLING");
        public static IncomeType Gift = new(5, "GIFT");
        private IncomeType(int id, string name) : base(id, name) { }
    }
}
