using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Services.Api
{
    public interface IQueryStore<out TReadModel>
    {
        TReadModel Get(Guid id);
    }
    public interface ICollectionQueryStore<out TReadModel> 
    {
        IEnumerable<TReadModel> GetAll(Guid id);
    }
}
