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
                .WithParameter($"ParentId", id.ToString());
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
        => await expense.Container.ReadItemAsync<ReadModels.Expense>(id.ToString(), new PartitionKey(id.ToString()));
        private async Task AddExpense(Events.ExpenseAddedToBudget e,Task budgetTask)
        {
            await budgetTask;
            await expense.Container.CreateItemAsync<ReadModels.Expense>(e.ToModel(),new PartitionKey(e.ExpenseId.ToString()));
        }
        private async Task UpdateExpense(Guid id,Guid expenseId, Action<ReadModels.Expense> operation,Task budgetTask)
        {
            await budgetTask;
            await Update(id, operation, expense.Container, new PartitionKey(expenseId.ToString()));
        }
        private async Task DeleteExpense(Events.ExpenseDeleted e, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await expense.Container.DeleteItemAsync<ReadModels.Expense>(e.ExpenseId.ToString(), new PartitionKey(e.ExpenseId.ToString()));
        }

    }
}
