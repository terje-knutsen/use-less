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
        /// Add an income to a budget
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-income")]
        public async Task<IActionResult> AddIncome(Guid budgetId, V1.AddIncome request)
        => await Handle(budgetId, request);
        /// <summary>
        /// Add an outgo to a budget
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-outgo")]
        public async Task<IActionResult> AddOutgo(Guid budgetId, V1.AddOutgo request)
        => await Handle(budgetId, request);
        /// <summary>
        /// Add an expense to a budget
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost,Route("{id}/add-expense")]
        public async Task<IActionResult> AddExpense(Guid budgetId, V1.AddExpense request)
        => await Handle(budgetId, request);
        /// <summary>
        /// Change an income amount
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        
        [HttpPut, Route("{id}/change-income-amount")]
        public async Task<IActionResult> ChangeIncomeAmount(Guid budgetId, V1.ChangeIncomeAmount request)
        => await Handle(budgetId, request);
        /// <summary>
        /// Change a income type
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-income-type")]
        public async Task<IActionResult> ChangeIncomeType(Guid budgetId, V1.ChangeIncomeType request)
        => await Handle(budgetId, request);
        ///<summary>
        ///Change an outgo amount
        ///</summary>
        ///<param name="budgetId"></param>
        ///<param name="request"></param>
        [HttpPut, Route("{id}/change-outgo-amount")]
        public async Task<IActionResult> ChangeOutgoAmount(Guid budgetId, V1.ChangeOutgoAmount request)
        => await Handle(budgetId, request);
        /// <summary>
        /// Change a outgo type
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}/change-outgo-type")]
        public async Task<IActionResult> ChangeOutgoType(Guid budgetId, V1.ChangeOutgoType request)
        => await Handle(budgetId, request);
        [HttpPut, Route("{id}/change-expense-amount")]
        public async Task<IActionResult> ChangeExpenseAmount(Guid budgetId, V1.ChangeExpenseAmount request)
            => await Handle(budgetId, request);
        private async Task<IActionResult> Handle(Guid budgetId, object request)
        {
            await applicationService.Handle(budgetId, request);
            return Ok();
        }


    }
}
