using System.Collections.ObjectModel;
using Useless.Mobile.Models;

namespace Useless.Mobile.Extensions
{
    public static class CollectionExtensions
    {
        public static void ReplaceItem<T>(this Collection<T> col, Func<T, bool> match, T newItem)
        {
            var oldItem = col.FirstOrDefault(i => match(i));
            var oldIndex = col.IndexOf(oldItem);
            col[oldIndex] = newItem;
        }
        public static int GetIndexOf(this ObservableCollection<Budget> budgets, Budget budget)
        {
            var existingBudget = budgets.Where(x => x.BudgetId == budget.BudgetId).FirstOrDefault();
            if (existingBudget != null)
            {
                return budgets.IndexOf(existingBudget);
            }
            return -1;
        }
    }
}
