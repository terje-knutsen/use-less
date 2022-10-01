using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryStore<ReadModels.Income>,
            ICollectionQueryStore<ReadModels.Income>
    {
        Task<ReadModels.Income> IQueryStore<ReadModels.Income>.Get(Guid id)
        {
            throw new NotImplementedException();
        }
        Task<IEnumerable<ReadModels.Income>> ICollectionQueryStore<ReadModels.Income>.GetAll(Guid id)
        {
            throw new NotImplementedException();
        }
        private Task AddIncome(Events.IncomeAddedToBudget e)
        {
            throw new NotImplementedException();
        }
        private Task ChangeIncomeAmount(Events.IncomeAmountChanged e)
        {
            throw new NotImplementedException();
        }
        private Task ChangeIncomeType(Events.IncomeTypeChanged e)
        {
            throw new NotImplementedException();
        }
        private Task DeleteIncome(Events.IncomeDeleted e)
        {
            throw new NotImplementedException();
        }
    }
}
