using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Api;
using UseLess.Domain;
using UseLess.Domain.Enumerations;
using UseLess.Domain.Values;
using UseLess.Framework;
using UseLess.Messages;
using UseLess.Services.Api;
using UseLess.Services.Extensions;
using static UseLess.Messages.BudgetCommands;

namespace UseLess.Services
{
    public class BudgetService : IApplicationService
    {
        private readonly IAggregateStore aggregateStore;
        private static EntryTime Now => EntryTime.From(DateTime.Now);
        public BudgetService(IAggregateStore aggregateStore)
        {
            this.aggregateStore = aggregateStore;
        }
        public Task Handle(object command)
        => command switch
        {
            V1.Create cmd => Create(cmd),
            V1.AddIncome cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.AddIncome(
                            IncomeId.From(cmd.IncomeId),
                            Money.From(cmd.Amount),
                            Enumeration.FromString<IncomeType>(cmd.Type),
                            Now)),
            V1.AddOutgo cmd =>
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.AddOutgo(
                            OutgoId.From(cmd.OutgoId),
                            Money.From(cmd.Amount),
                            Enumeration.FromString<OutgoType>(cmd.Type),
                            Now)),
            V1.AddExpense cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.AddExpense(
                            ExpenseId.From(cmd.ExpenseId),
                            Money.From(cmd.Amount),
                            Now)),
            V1.AddPeriod cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.AddPeriod(
                            PeriodId.From(cmd.PeriodId),
                            StartTime.From(cmd.StartTime),
                            Now)),
            V1.ChangeIncomeAmount cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.ChangeIncomeAmount(
                        IncomeId.From(cmd.IncomeId),
                        Money.From(cmd.Amount),
                        Now)),
            V1.ChangeIncomeType cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.ChangeIncomeType(
                        IncomeId.From(cmd.IncomeId),
                        Enumeration.FromString<IncomeType>(cmd.Type),
                        Now)),
            V1.ChangeOutgoAmount cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.ChangeOutgoAmount(
                        OutgoId.From(cmd.OutgoId),
                        Money.From(cmd.Amount),
                        Now)),
            V1.ChangeOutgoType cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.ChangeOutgoType(
                        OutgoId.From(cmd.OutgoId),
                        Enumeration.FromString<OutgoType>(cmd.Type),
                        Now)),
            V1.ChangeExpenseAmount cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.ChangeExpenseAmount(
                        ExpenseId.From(cmd.ExpenseId),
                        Money.From(cmd.Amount),
                        Now)),
            V1.SetPeriodState cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.SetPeriodState(
                        Enumeration.FromString<PeriodState>(cmd.PeriodState),
                        Now)),
            V1.SetPeriodStopTime cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.SetPeriodStop(
                        StopTime.From(cmd.StopTime),
                        Now)),
            V1.SetPeriodType cmd => 
                HandleUpdate(
                    cmd.BudgetId,
                    x => x.SetPeriodType(
                        Enumeration.FromString<PeriodType>(cmd.PeriodType),
                        Now)),
            _ => throw new InvalidOperationException("No such command")

        };
        private async Task Create(V1.Create cmd) 
        {
            if (await aggregateStore.Exists<Budget, BudgetId>(BudgetId.From(cmd.BudgetId)))
                throw new InvalidOperationException($"Entity {cmd.Name} already exist");
            await aggregateStore.Save<Budget,BudgetId>(
                Budget.Create(BudgetId.From(cmd.BudgetId), BudgetName.From(cmd.Name)));
        }
        private async Task HandleUpdate(Guid budgetId, Action<Budget> action)
            => await this.Update(aggregateStore, BudgetId.From(budgetId), action);
    }
}
