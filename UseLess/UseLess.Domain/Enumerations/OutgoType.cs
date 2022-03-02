using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class OutgoType : Enumeration
    {
        public static OutgoType Unexpected = new OutgoType(1, "UNEXPECTED");
        public static OutgoType Yearly = new OutgoType(2, "YEARLY");
        public static OutgoType Monthly = new OutgoType(3, "MONTHLY");
        public static OutgoType Weekly = new OutgoType(4, "WEEKLY");
        public OutgoType(int id, string name) : base(id, name)
        { }
    }
}
