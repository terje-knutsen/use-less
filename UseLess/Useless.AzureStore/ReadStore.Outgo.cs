using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore :
            IQueryStore<ReadModels.Outgo>,
            ICollectionQueryStore<ReadModels.Outgo>
    {
        Task<ReadModels.Outgo> IQueryStore<ReadModels.Outgo>.Get(Guid id)
        {
            throw new NotImplementedException();
        }
        Task<IEnumerable<ReadModels.Outgo>> ICollectionQueryStore<ReadModels.Outgo>.GetAll(Guid id)
        {
            throw new NotImplementedException();
        }
        private Task AddOutgo(Events.OutgoAddedToBudget e)
        {
            throw new NotImplementedException();
        }
        private Task ChangeOutgoAmount(Events.OutgoAmountChanged e)
        {
            throw new NotImplementedException();
        }
        private Task DeleteOutgo(Events.OutgoDeleted e)
        {
            throw new NotImplementedException();
        }
        private Task ChangeOutgoType(Events.OutgoTypeChanged e)
        {
            throw new NotImplementedException();
        }
    }
}
