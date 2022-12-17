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
            IQueryStore<ReadModels.Outgo>,
            ICollectionQueryStore<ReadModels.Outgo>
    {
        private ContainerResponse _outgo;
        async Task<ReadModels.Outgo> IQueryStore<ReadModels.Outgo>.Get(Guid id)
        => await budgetContainer.Container.ReadItemAsync<ReadModels.Outgo>(id.ToString(),new PartitionKey(id.ToString()));
        async Task<IEnumerable<ReadModels.Outgo>> ICollectionQueryStore<ReadModels.Outgo>.GetAll(Guid id)
        {
            var items = new List<ReadModels.Outgo>();
            IReadOnlyList<FeedRange> feedRange = await _outgo.Container.GetFeedRangesAsync();
            var queryText = $@"
                SELECT * FROM {nameof(ReadModels.Outgo)}
                WHERE {nameof(ReadModels.Outgo)}.{nameof(ReadModels.Outgo.ParentId)} = @ParentId";

            var queryDefinition = new QueryDefinition(queryText)
                .WithParameter($"{nameof(ReadModels.Outgo.ParentId)}",id.ToString());
            using(var feedIterator = _outgo.Container.GetItemQueryIterator<ReadModels.Outgo>(feedRange[0],queryDefinition))
            {
                while(feedIterator.HasMoreResults)
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                        items.Add(item);
                }
                return items;
            }

        }
        private async Task AddOutgo(Events.OutgoAddedToBudget e, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await _outgo.Container.CreateItemAsync<ReadModels.Outgo>(e.ToModel(),new PartitionKey(e.OutgoId.ToString())); 
        }
        private async Task UpdateOutgoAsync(Guid id, Guid outgoId, Action<ReadModels.Outgo> operation, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await Update(id, operation, _outgo, new PartitionKey(outgoId.ToString()));
        }
        private async Task DeleteOutgo(Events.OutgoDeleted e, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            await _outgo.Container.DeleteItemAsync<ReadModels.Outgo>(e.OutgoId.ToString(),new PartitionKey(e.OutgoId.ToString()));
        }

    }
}
