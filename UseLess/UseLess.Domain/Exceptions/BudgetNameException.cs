using UseLess.Domain.Exceptions.Base;

namespace UseLess.Domain.Exceptions
{
    public sealed class BudgetNameException : BudgetException
    {
        public BudgetNameException(string message):base(message)
        { }
    }
}
