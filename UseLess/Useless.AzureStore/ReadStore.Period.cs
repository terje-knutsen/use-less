using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryStore<ReadModels.Period>
    {
        private ContainerResponse _period;
        async Task<ReadModels.Period> IQueryStore<ReadModels.Period>.Get(Guid id)
        => await _period.Container.ReadItemAsync<ReadModels.Period>(id.ToString(), new PartitionKey(id.ToString()));
        private async Task AddPeriod(Events.PeriodCreated e, Task budgetTask)
        {
            await budgetTask;
            await _period.Container.CreateItemAsync<ReadModels.Period>(e.ToModel(), new PartitionKey(e.PeriodId.ToString()));  
        }
        private async Task UpdatePeriod(Guid id, Guid periodId, Action<ReadModels.Period> operation, params Task[] tasks)
        {
            foreach (var task in tasks)
                await task;
            await Update(periodId, operation, _period.Container,new PartitionKey(periodId.ToString()));
        }
    }
}
