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

        public AggregateStore(IReadStream streamReader, IWriteToStream streamWriter)
        {
            this.streamReader = streamReader;
            this.streamWriter = streamWriter;
        }
        public async Task<bool> Exists<T, TId>(TId aggregateId)
        {
            if (aggregateId == null) return false;
            var header = await streamReader.ReadHeader(aggregateId.ToString());
            return header != null;
        }

        public async Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>
        {
            if (aggregateId != null) 
            {
                StreamResponse streamResponse = await streamReader.ReadStream(aggregateId.ToString());
                var stream = streamResponse.Stream;
                if(stream.HasValue) 
                {
                    var eventDatas = stream.Value.Events.Select(x => x.Body).ToArray();
                    var aggregate = Activator.CreateInstance(typeof(T), eventDatas) as T;
                    if (aggregate == null)
                        throw new InvalidDataException("Aggregate could not be hydrated");
                    return aggregate;
                }
            }
            throw new InvalidDataException("Aggregate could not be loaded");
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
                    await streamWriter.WriteToStream(id, events);
                }
            }
        }
        private EventData[] GetEventsToWrite(string streamId,ulong version,IEnumerable<object> objects) 
           => objects.Select(x => new EventData(streamId, x, nameof(x), version, DateTime.Now.ToString("yy.MM.dd HH:mm"))).ToArray();
    }
}
