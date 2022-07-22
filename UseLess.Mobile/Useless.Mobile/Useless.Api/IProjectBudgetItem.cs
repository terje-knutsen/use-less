using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Useless.Api
{
    public interface IProjection<T>
    {
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(Guid id = default);
    }
}
