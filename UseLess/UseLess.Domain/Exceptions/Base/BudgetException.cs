
namespace UseLess.Domain.Exceptions.Base
{
    public abstract class BudgetException : Exception
    {
        public BudgetException(string message):base(message)
        { }
    }
}
