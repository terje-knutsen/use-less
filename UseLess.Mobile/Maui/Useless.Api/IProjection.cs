using System.Collections.Generic;
using System.Threading.Tasks;

namespace Useless.Api
{
    public interface IProjection<T,K>
    {
        Task<T> GetAsync(K queryModel);
    }
}
