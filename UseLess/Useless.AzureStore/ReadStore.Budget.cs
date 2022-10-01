using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Enumerations;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryStore<ReadModels.Budget>
    {
        private ContainerResponse budgetContainer;
        public async Task<ReadModels.Budget> Get(Guid id)
        => await budgetContainer.Container.ReadItemAsync<ReadModels.Budget>(id.ToString(), new PartitionKey(BudgetState.Active.Name));
        private async Task CreateBudget(Events.BudgetCreated e)
         =>  await budgetContainer.Container.CreateItemAsync<ReadModels.Budget>(e.ToModel(), new PartitionKey(e.State));
        private async Task DeleteBudget(Events.BudgetDeleted e)
        => await budgetContainer.Container.DeleteItemAsync<ReadModels.Budget>(e.Id.ToString(), new PartitionKey(e.State));
        private async Task UpdateBudget(Guid id, Action<ReadModels.Budget> operation)
        {
            var budget = await budgetContainer.Container.ReadItemAsync<ReadModels.Budget>(id.ToString(), new PartitionKey(BudgetState.Active.Name));
            if(budget != null) 
            {
                operation(budget);
                await budgetContainer.Container.UpsertItemAsync<ReadModels.Budget>(budget, new PartitionKey(BudgetState.Active.Name));
            }
        }
    }
}
