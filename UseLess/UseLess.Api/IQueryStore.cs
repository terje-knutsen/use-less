using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Services.Api
{
    public interface IQueryStore<TReadModel>
    {
        Task<TReadModel> Get(Guid id);
    }
    public interface ICollectionQueryStore<TReadModel> 
    {
        Task<IEnumerable<TReadModel>> GetAll(Guid id);
    }
}
