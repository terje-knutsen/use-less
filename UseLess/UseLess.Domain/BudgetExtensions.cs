using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Entities;
using UseLess.Domain.Values;

namespace UseLess.Domain
{
    public static class BudgetExtensions
    {
        public static Income ById(this IEnumerable<Income> incomes,IncomeId id) 
        {
            var income = incomes.FirstOrDefault(x => x.Id == id);
            if (income == null)
                throw new InvalidOperationException("Income does not exist");
            return income;
        }
        public static Outgo ById(this IEnumerable<Outgo> outgos, OutgoId id) 
        {
            var outgo = outgos.FirstOrDefault(x => x.Id == id);
            if (outgo == null)
                throw new InvalidOperationException("Outgo does not exiet");
            return outgo;
        }
    }
}
