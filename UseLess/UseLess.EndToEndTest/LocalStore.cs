using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;
using UseLess.Services.Api;

namespace UseLess.EndToEndTest
{
    public class LocalStore : IAggregateStore
    {
        public Task<bool> Exists<T, TId>(TId aggregateId)
        => Task.Factory.StartNew(()=> keyValuePairs.ContainsKey($"{typeof(T).Name}-{aggregateId}"));

        public Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>
        {
            var events = keyValuePairs[$"{typeof(T).Name}-{aggregateId}"];
            var aggregate = Activator.CreateInstance(typeof(T), events) as T;
            if (aggregate == null)
                throw new InvalidDataException("Aggregate does not exist");
            return Task.Factory.StartNew(()=>  aggregate);
        }

        public Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId>
        {
            var changes = aggregate.GetChanges();
            return AddEvents<T, TId>(aggregate.Id, aggregate.GetChanges());
        }

        private Task AddEvents<T, TId>(TId id, IEnumerable<object> events) where T : AggregateRoot<TId>
        {
            var key = $"{typeof(T).Name}-{id}";
            Action action;
            if (keyValuePairs.ContainsKey(key))
            {
                action = () =>
                {
                    var existingEvents = keyValuePairs[key].ToList();
                    existingEvents.AddRange(events);
                    keyValuePairs[key] = existingEvents;
                };
            }
            else 
                action = () => keyValuePairs.Add($"{typeof(T).Name}-{id}", events);
            return Task.Factory.StartNew(() => action());
        }

        private IDictionary<string, IEnumerable<object>> keyValuePairs = new Dictionary<string, IEnumerable<object>>();
    }
}
