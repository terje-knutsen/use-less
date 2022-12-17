using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;

namespace UseLess.EndToEndTest
{
    public partial class LocalStore
    {
        private readonly IList<ReadModels.Period> periods = new List<ReadModels.Period>();
        private async Task AddPeriod(Events.PeriodCreated e, params Task[] tasks) 
        {
            await Task.WhenAll(tasks);
            CreatePeriod(e);
        }
        private async Task ChangePeriodType(Events.PeriodTypeChanged e)
            => await UpdatePeriod(e.Id, x => x.Type = e.PeriodType);
        private async Task ChangePeriodState(Events.PeriodStateChanged e)
            => await UpdatePeriod(e.Id, x => x.State = e.State);
        private async Task UpdatePeriod(Guid periodId, Action<ReadModels.Period> action, params Task[] tasks)
        {
            await Task.WhenAll(tasks);
            var period = periods.First(x => x.PeriodId == periodId.ToString());
            action(period);
        }
        private void CreatePeriod(Events.PeriodCreated e)
            => periods.Add(new ReadModels.Period 
            {
                PeriodId = e.PeriodId.ToString(),
                ParentId = e.Id.ToString(),
                Start = e.Start,
                Stop = e.Stop,
                State = e.State,
                Type  = e.Type
            });
    }
}
