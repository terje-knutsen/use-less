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
            return Task.Factory.StartNew(() => 
            keyValuePairs.Add($"{typeof(T).Name}-{aggregate.Id}", aggregate.GetChanges()));
        }

        private IDictionary<string, IEnumerable<object>> keyValuePairs = new Dictionary<string, IEnumerable<object>>();
    }
}
