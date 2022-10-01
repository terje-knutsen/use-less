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
    public partial class ReadStore : 
        IQueryStore<ReadModels.Expense>, 
        ICollectionQueryStore<ReadModels.Expense>
    {
        private ContainerResponse expense;
        public async Task<IEnumerable<ReadModels.Expense>> GetAll(Guid id)
        {
            var items = new List<ReadModels.Expense>();
            IReadOnlyList<FeedRange> feedRange = await expense.Container.GetFeedRangesAsync();

            var queryText = $@"
                        SELECT * FROM {nameof(ReadModels.Expense)}
                        WHERE {nameof(ReadModels.Expense)}.{nameof(ReadModels.Expense.ParentId)} = @ParentId";
            var queryDefinition = new QueryDefinition(queryText)
                .WithParameter($"{nameof(ReadModels.Expense.ParentId)}", id.ToString());
            using (var feedIterator = expense.Container.GetItemQueryIterator<ReadModels.Expense>(
                feedRange[0], queryDefinition, null, new QueryRequestOptions() { })) 
            {
                while (feedIterator.HasMoreResults) 
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                        items.Add(item);
                }
                return items;
            }
        }
        async Task<ReadModels.Expense> IQueryStore<ReadModels.Expense>.Get(Guid id)
        => await expense.Container.ReadItemAsync<ReadModels.Expense>(id.ToString(), PartitionKey.Null);
        private async Task AddExpense(Events.ExpenseAddedToBudget e,Task budgetTask)
        {
            await budgetTask;
            await expense.Container.CreateItemAsync<ReadModels.Expense>(e.ToModel());
        }
        private async Task ChangeExpenseAmount(Guid id, Action<ReadModels.Expense> operation,Task budgetTask)
        {
            await budgetTask;
            var item = await expense.Container.ReadItemAsync<ReadModels.Expense>(id.ToString(), PartitionKey.Null);
            if (item != null) 
            {
                operation(item);
                await expense.Container.UpsertItemAsync<ReadModels.Expense>(item);
            }
        }
        private async Task DeleteExpense(Events.ExpenseDeleted e)
        => await expense.Container.DeleteItemAsync<ReadModels.Expense>(e.Id.ToString(), PartitionKey.Null);
    }
}
