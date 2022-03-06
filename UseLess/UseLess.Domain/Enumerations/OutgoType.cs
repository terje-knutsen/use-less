using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class OutgoType : Enumeration
    {
        public static OutgoType Unexpected = new(1, "UNEXPECTED");
        public static OutgoType Yearly = new(2, "YEARLY");
        public static OutgoType Monthly = new(3, "MONTHLY");
        public static OutgoType Weekly = new(4, "WEEKLY");
        public OutgoType(int id, string name) : base(id, name)
        { }
    }
}
