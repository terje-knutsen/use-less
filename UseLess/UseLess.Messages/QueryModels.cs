using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Messages
{
    public static class QueryModels
    {
        public class GetBudget 
        {
            public Guid BudgetId { get; set; }
        }
    }
}
