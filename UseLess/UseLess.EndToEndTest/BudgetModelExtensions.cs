using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;

namespace UseLess.EndToEndTest
{
    public static class BudgetModelExtensions
    {
        public static ReadModels.Budget ToModel(this Events.BudgetCreated @event)
            => new ReadModels.Budget
            {
                BudgetId = @event.Id,
                Name = @event.Name,
                EntryTime = @event.EntryTime,
                State = @event.State
            };
    }
}
