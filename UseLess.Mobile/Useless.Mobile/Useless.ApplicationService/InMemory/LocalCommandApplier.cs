using System;
using System.Threading.Tasks;
using Useless.Api;
using UseLess.Messages;
using static UseLess.Messages.BudgetCommands;

namespace Useless.ApplicationService.InMemory
{
    public sealed class LocalCommandApplier : IApplyBudgetCommand
    {
        public Task Apply(Guid id, object cmd)
        => cmd switch
        {
            V1.Create => AddBudget(cmd),
            V1.AddIncome => AddIncome(id,cmd),
            V1.ChangeIncomeAmount => ChangeIncomeAmount(id,cmd),
            V1.AddOutgo => AddOutgo(id,cmd),
            V1.ChangeOutgoAmount => ChangeOutgoAmount(id,cmd as V1.ChangeOutgoAmount),
        };

        

        private async Task AddBudget(object cmd)
        {
            if(cmd is V1.Create createCommand)
            {
              await Task.Run(()=> Budgets.Instance.Add(new ReadModels.Budget { BudgetId = createCommand.BudgetId, Name = createCommand.Name }));
            }
        }
        private async Task AddIncome(Guid id, object cmd)
        {
            if(cmd is V1.AddIncome addIncomeCommand)
            {
                var income = new ReadModels.Income
                {
                    IncomeId = addIncomeCommand.IncomeId,
                    ParentId = id,
                    Amount = addIncomeCommand.Amount,
                    Type = addIncomeCommand.Type,
                    EntryTime = DateTime.Now
                };
                await Task.Run(()=> Incomes.Instance.Add(income));
                await UpdateBudgetIncome(id, addIncomeCommand.Amount);
            }
        }
        private async Task ChangeIncomeAmount(Guid id, object cmd)
        {
            if (cmd is V1.ChangeIncomeAmount changeIncomeAmount)
            {
                await UpdateBudgetIncome(id, changeIncomeAmount.Amount);
            }
        }

        private static async Task UpdateBudgetIncome(Guid id, decimal amount)
        {
            var item = Budgets.Instance[id];
            await Task.Run(() =>
            {
                item.Income += amount;
                item.Left += amount;
                item.Available += amount / 30;
                item.Limit += amount / 30;
            });
        }

        private async Task AddOutgo(Guid id, object cmd)
        {
            if(cmd is V1.AddOutgo addOutgoCommand)
            {
                var outgo = new ReadModels.Outgo
                {
                    Amount = addOutgoCommand.Amount,
                    EntryTime = DateTime.Now,
                    OutgoId = addOutgoCommand.OutgoId,
                    ParentId = id,
                    Type = addOutgoCommand.Type
                };
                await Task.Run(() => 
                {
                    Outgos.Instance.Add(outgo);
                });
            }
        }
        private async Task ChangeOutgoAmount(Guid id, V1.ChangeOutgoAmount cmd) 
        {
            var item = Outgos.Instance[cmd.OutgoId];
            await Task.Run(() => item.Amount = cmd.Amount);
        }
    }
}
