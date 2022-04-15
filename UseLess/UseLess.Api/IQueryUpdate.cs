using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Services.Api
{
    public interface IQueryUpdate
    {
        Task Update(IEnumerable<object> events);
    }
}
