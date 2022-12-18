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
    public partial class ReadStore : IQueryStore<ReadModels.Income>,
            ICollectionQueryStore<ReadModels.Income>
    {
        
        private ContainerResponse income;
        async Task<ReadModels.Income> IQueryStore<ReadModels.Income>.Get(Guid id)
        => await income.Container.ReadItemAsync<ReadModels.Income>(id.ToString(), new PartitionKey(id.ToString()));
        async Task<IEnumerable<ReadModels.Income>> ICollectionQueryStore<ReadModels.Income>.GetAll(Guid id)
        {
            var items = new List<ReadModels.Income>();
            IReadOnlyList<FeedRange> feedRange = await income.Container.GetFeedRangesAsync();

            var queryText = $@"
                    SELECT * FROM {nameof(ReadModels.Income)}
                    WHERE {nameof(ReadModels.Income)}.{nameof(ReadModels.Income.ParentId)} = @ParentId";
            var queryDefinition = new QueryDefinition(queryText)
                .WithParameter($"@ParentId", id.ToString());
            using(var feedIterator = income.Container.GetItemQueryIterator<ReadModels.Income>(feedRange[0], queryDefinition))
            {
                while (feedIterator.HasMoreResults) 
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                        items.Add(item);  
                }
                return items;
            }
        }
        private async Task AddIncome(Events.IncomeAddedToBudget e, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await income.Container.CreateItemAsync<ReadModels.Income>(e.ToModel(),new PartitionKey(e.IncomeId.ToString()));
        }
        private async Task UpdateIncome(Guid id,Guid incomeId, Action<ReadModels.Income> operation, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await Update<ReadModels.Income>(id, operation, income.Container, new PartitionKey(incomeId.ToString())); 
        }

        private async Task DeleteIncome(Events.IncomeDeleted e, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await income.Container.DeleteItemAsync<ReadModels.Income>(e.IncomeId.ToString(), new PartitionKey(e.IncomeId.ToString()));
        }
    }
}
