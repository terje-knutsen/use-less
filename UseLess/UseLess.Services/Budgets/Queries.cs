using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace UseLess.Services.Budgets
{
    public sealed class Queries : IQueryService<ReadModels.Budget>
    {
        private readonly IQueryStore<ReadModels.Budget> queryStore;

        public Queries(IQueryStore<ReadModels.Budget> queryStore)
        {
            this.queryStore = queryStore;
        }
        public ReadModels.Budget Query(object query)
        => query switch
        {
            QueryModels.GetBudget x =>
                queryStore.Get(x.BudgetId),
        };
    }
}
