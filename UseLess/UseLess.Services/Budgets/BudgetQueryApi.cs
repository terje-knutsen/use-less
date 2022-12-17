using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;
using UseLess.Services.Api;

namespace UseLess.Services.Budgets
{
    [ApiController,Route("/budget")]
    public sealed class BudgetQueryApi : ControllerBase
    {
        private static readonly ILogger log = Log.ForContext<ReadModels.Budget>();
        private readonly IBudgetQueryService query;

        public BudgetQueryApi(IBudgetQueryService queryService)
        {
            query = queryService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetBudget request)
        => await RequestHandler.HandleQuery(() => query.GetBudget(request),log);
        [HttpGet, Route("incomes")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetIncomes request)
            => await RequestHandler.HandleQuery(() => query.GetIncomes(request), log);
        [HttpGet, Route("outgos")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetOutgos request) => await RequestHandler.HandleQuery(() => query.GetOutgos(request), log);
        [HttpGet, Route("expenses")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetExpenses request) => await RequestHandler.HandleQuery(() => query.GetExpenses(request), log);
        [HttpGet, Route("income")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetIncome request) => await RequestHandler.HandleQuery(() => query.GetIncome(request), log);
        [HttpGet, Route("outgo")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetOutgo request) => await RequestHandler.HandleQuery(() => query.GetOutgo(request), log);
        [HttpGet, Route("expense")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetExpense request) => await RequestHandler.HandleQuery(() => query.GetExpense(request), log);
        [HttpGet, Route("period")]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetPeriod request) => await RequestHandler.HandleQuery(() => query.GetPeriod(request), log);
    }
}
