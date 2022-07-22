using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Useless.Api;
using UseLess.Messages;

namespace Useless.ApplicationService.InMemory
{
    public sealed class LocalProjection :
        IProjection<ReadModels.Budget>,
        IProjection<ReadModels.Income>,
        IProjection<ReadModels.Outgo>,
        IProjection<ReadModels.Expense>
    {
        public Task<IEnumerable<ReadModels.Budget>> GetAllAsync(Guid id = default)
        => Task.Run(() => Budgets.Instance.All);
        public Task<ReadModels.Budget> GetAsync(Guid id)
        => Task.Run(() => Budgets.Instance[id]);
        Task<IEnumerable<ReadModels.Income>> IProjection<ReadModels.Income>.GetAllAsync(Guid id)
        => Task.Run(() => Incomes.Instance.All(id));
        Task<ReadModels.Income> IProjection<ReadModels.Income>.GetAsync(Guid id)
        => Task.Run(() => Incomes.Instance[id]);
        Task<IEnumerable<ReadModels.Outgo>> IProjection<ReadModels.Outgo>.GetAllAsync(Guid id)
        => Task.Run(() => Outgos.Instance.All(id));
        Task<ReadModels.Outgo> IProjection<ReadModels.Outgo>.GetAsync(Guid id)
        => Task.Run(() => Outgos.Instance[id]);

        Task<ReadModels.Expense> IProjection<ReadModels.Expense>.GetAsync(Guid id)
        => Task.Run(() => Expenses.Instance[id]);

        Task<IEnumerable<ReadModels.Expense>> IProjection<ReadModels.Expense>.GetAllAsync(Guid id)
        => Task.Run(() => Expenses.Instance.All(id));
    }
}
