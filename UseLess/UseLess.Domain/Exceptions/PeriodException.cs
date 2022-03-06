using UseLess.Domain.Exceptions.Base;

namespace UseLess.Domain.Exceptions
{
    public sealed class PeriodException : BudgetException
    {
        public PeriodException(string message) : base(message)
        { }
    }
}
