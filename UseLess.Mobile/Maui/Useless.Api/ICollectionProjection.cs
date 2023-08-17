using System.Collections.Generic;
using System.Threading.Tasks;

namespace Useless.Api
{
    public interface ICollectionProjection<T,K>
    {
        Task<IEnumerable<T>> GetAsync(K queryModel);
    }
}
