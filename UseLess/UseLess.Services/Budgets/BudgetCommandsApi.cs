using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Api;
using static UseLess.Messages.BudgetCommands;

namespace UseLess.Services.Budgets
{
    [Route("api/budget")]
    [ApiController]
    public class BudgetCommandsApi : ControllerBase
    {
        private readonly IApplicationService applicationService;
        public BudgetCommandsApi(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(V1.Create request)
        => await Handle(request.BudgetId, request);
        /// <summary>
        /// Add income to a budget
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-income")]
        public async Task<IActionResult> AddIncome(Guid id, V1.AddIncome request)
        => await Handle(id, request);
        /// <summary>
        /// Add outgo to a budget
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-outgo")]
        public async Task<IActionResult> AddOutgo(Guid id, V1.AddOutgo request)
        => await Handle(id, request);
        /// <summary>
        /// Add expense to a budget
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-expense")]
        public async Task<IActionResult> AddExpense(Guid id, V1.AddExpense request)
        => await Handle(id, request);

        /// <summary>
        /// Add period
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/add-period")]
        public async Task<IActionResult> AddPeriod(Guid id, V1.AddPeriod request)
            => await Handle(id, request);
        /// <summary>
        /// Change income amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-income-amount")]
        public async Task<IActionResult> ChangeIncomeAmount(Guid id, V1.ChangeIncomeAmount request)
        => await Handle(id, request);
        /// <summary>
        /// Change income type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-income-type")]
        public async Task<IActionResult> ChangeIncomeType(Guid id, V1.ChangeIncomeType request)
        => await Handle(id, request);
        ///<summary>
        ///Change outgo amount
        ///</summary>
        ///<param name="id"></param>
        ///<param name="request"></param>
        [HttpPut, Route("{id}/change-outgo-amount")]
        public async Task<IActionResult> ChangeOutgoAmount(Guid id, V1.ChangeOutgoAmount request)
        => await Handle(id, request);
        /// <summary>
        /// Change outgo type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-outgo-type")]
        public async Task<IActionResult> ChangeOutgoType(Guid id, V1.ChangeOutgoType request)
        => await Handle(id, request);
        /// <summary>
        /// Change expense amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-expense-amount")]
        public async Task<IActionResult> ChangeExpenseAmount(Guid id, V1.ChangeExpenseAmount request)
            => await Handle(id, request);
        /// <summary>
        /// Set period state
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/set-period-state")]
        public async Task<IActionResult> SetPeriodState(Guid id, V1.SetPeriodState request)
            => await Handle(id, request);
        /// <summary>
        /// Set period stop time
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/set-period-stop-time")]
        public async Task<IActionResult> SetPeriodStopTime(Guid id, V1.SetPeriodStopTime request)
            => await Handle(id, request);
        /// <summary>
        /// Set period type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/set-period-type")]
        public async Task<IActionResult> SetPeriodType(Guid id, V1.SetPeriodType request)
            => await Handle(id, request);
        /// <summary>
        /// Delete income
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}/income")]
        public async Task<IActionResult> DeleteIncome(Guid id, V1.DeleteIncome request)
            => await Handle(id, request);
        /// <summary>
        /// Delete outgo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}/outgo")]
        public async Task<IActionResult> DeleteOutgo(Guid id, V1.DeleteBudget request)
            => await Handle(id, request);
        /// <summary>
        /// Delete expense
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}/expense")]
        public async Task<IActionResult> DeleteExpense(Guid id, V1.DeleteExpense request)
            => await Handle(id, request);
        /// <summary>
        /// Delete budget
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}/budget")]
        public async Task<IActionResult> DeleteBudget(Guid id, V1.DeleteBudget request)
            => await Handle(id, request);
 
        private async Task<IActionResult> Handle(Guid id, object request)
        {
            await applicationService.Handle(id, request);
            return Ok();
        }


    }
}
