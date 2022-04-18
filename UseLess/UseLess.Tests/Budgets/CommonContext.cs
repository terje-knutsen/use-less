using Moq;
using SpecsFor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Domain.Values;

namespace UseLess.Domain.Tests.Budgets
{
    internal static class CommonContext
    {
        internal class PeriodIsSet : IContext<Budget>
        {
            private readonly StartTime startTime;

            public PeriodIsSet(StartTime startTime)
            {
                this.startTime = startTime;
            }
            public void Initialize(ISpecs<Budget> state)
            {
                var periodId = PeriodId.From(Guid.NewGuid());
                state.SUT.AddPeriod(periodId, startTime, It.IsAny<EntryTime>());
                state.SUT.SetPeriodStop(StopTime.From(((DateTime)startTime).AddDays(10)), It.IsAny<EntryTime>());
            }
        }
    }
}
