using Eveneum;
using Eveneum.Serialization;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public sealed class AggregateStore : IAggregateStore
    {
        private readonly IReadStream streamReader;
        private readonly IWriteToStream streamWriter;
        private readonly IQueryUpdate queryUpdater;

        public AggregateStore(IReadStream streamReader, IWriteToStream streamWriter,IQueryUpdate queryUpdater)
        {
            this.streamReader = streamReader;
            this.streamWriter = streamWriter;
            this.queryUpdater = queryUpdater;
        }
        public async Task<bool> Exists<T, TId>(TId aggregateId)
        {
            if (aggregateId == null) return false;
            var response = await streamReader.ReadStream(aggregateId.ToString(), new ReadStreamOptions { MaxItemCount = 1 });
            //var header = await streamReader.ReadHeader(aggregateId.ToString());
            return response.Stream != null;
        }

        public async Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>
        {
            if (aggregateId == null)
                throw new ArgumentException($"Argument {nameof(TId)} is null");
           
            StreamResponse streamResponse = await streamReader.ReadStream(aggregateId.ToString());
            var stream = streamResponse.Stream;
            var events = stream.HasValue ? stream.Value.Events : new EventData[0];    
            
            if(events == null || events.Length == 0)
                throw new InvalidDataException("Events does not exist");
            
             var eventDatas = events.Select(x => x.Body);
             var aggregate = Activator.CreateInstance(typeof(T), eventDatas) as T;

             if (aggregate == null || aggregate.Id == null)
                throw new InvalidDataException("Aggregate could not be hydrated");
                        
            return aggregate;
        }

        public async Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId>
        {
            if(aggregate != null) 
            {
                var changes = aggregate.GetChanges();
                var version = aggregate?.Version ?? 0;
                var id = aggregate?.Id?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(id))
                {
                    var events = GetEventsToWrite(id, version, changes);
                    await streamWriter.WriteToStream(id, events, version);
                    await queryUpdater.Update(changes);
                }
            }
        }
        private EventData[] GetEventsToWrite(string streamId,ulong version,IEnumerable<object> objects) 
           => objects.Select(x => new EventData(
               streamId, 
               x, 
               nameof(x), 
               ++version, 
               ((dynamic)x).EntryTime.ToString("yy.MM.dd HH:mm"))).ToArray();
    }
}
