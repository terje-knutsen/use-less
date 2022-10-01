using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace Useless.AzureStore
{
    public partial class ReadStore : IQueryStore<ReadModels.Period>
    {
        Task<ReadModels.Period> IQueryStore<ReadModels.Period>.Get(Guid id)
        {
            throw new NotImplementedException();
        }
        private Task AddPeriod(Events.PeriodCreated e, Task task)
        {
            throw new NotImplementedException();
        }
        private Task UpdatePeriod(Guid periodId, Action<ReadModels.Period> action, params Task[] tasks)
        {
            throw new NotImplementedException();
        }
        private Task ChangePeriodType(Events.PeriodTypeChanged e)
        {
            throw new NotImplementedException();
        }
        private Task ChangePeriodState(Events.PeriodStateChanged e)
        {
            throw new NotImplementedException();
        }
    }
}
