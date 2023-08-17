using System;
using System.Threading.Tasks;
namespace Useless.Api
{
    public interface IApplyBudgetCommand
    {
        Task Apply(Guid id, object cmd);
    }
}
