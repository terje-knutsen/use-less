namespace UseLess.Messages
{
    public static class Exceptions
    {
        public sealed class InvalidStateException : Exception 
        {
            private InvalidStateException(string message):base(message)
            { }
            public static InvalidStateException WithMessage(string message) => new(message);
        }
        public sealed class OutgoAlreadyExistException : Exception 
        {
            private OutgoAlreadyExistException(string message) : base(message) { }
            public static OutgoAlreadyExistException WithMessage(string message) => new(message);
            public static OutgoAlreadyExistException New => new("Outgo already added");
        }
        public sealed class PeriodException : Exception
        {
            private PeriodException(string message) : base(message) { }
            public static PeriodException WithMessage(string msg) => new(msg);
        }
        public sealed class BudgetNameException :Exception
        {
            private BudgetNameException(string message) : base(message) { }
            public static BudgetNameException WithMessage(String msg) => new(msg);
        }
    }
}
