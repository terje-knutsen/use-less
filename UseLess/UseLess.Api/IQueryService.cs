using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Services.Api
{
    public interface IQueryService<TReadModel>
    {
        TReadModel Query(object query);
    }
}
