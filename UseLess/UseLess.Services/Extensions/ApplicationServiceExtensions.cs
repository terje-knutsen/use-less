using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Api;
using UseLess.Framework;
using UseLess.Services.Api;

namespace UseLess.Services.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static async Task Update<T,TId>(this IApplicationService service, IAggregateStore store, TId aggregateId, Action<T> action)
            where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T,TId>(aggregateId);
            if (aggregate == null)
                throw new InvalidOperationException($"Entity with id {aggregateId} cannot be found");
            action(aggregate);
            await store.Save<T, TId>(aggregate);
        }
    }
}
