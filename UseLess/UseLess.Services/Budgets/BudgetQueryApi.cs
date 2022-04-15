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
        public IActionResult Get([FromQuery] QueryModels.GetBudget request)
        => RequestHandler.HandleQuery(() => query.GetBudget(request),log);
        [HttpGet, Route("incomes")]
        public IActionResult Get([FromQuery] QueryModels.GetIncomes request)
            => RequestHandler.HandleQuery(() => query.GetIncomes(request), log);
        [HttpGet, Route("outgos")]
        public IActionResult Get([FromQuery] QueryModels.GetOutgos request)
            => RequestHandler.HandleQuery(() => query.GetOutgos(request), log);
        [HttpGet, Route("expenses")]
        public IActionResult Get([FromQuery] QueryModels.GetExpenses request)
            => RequestHandler.HandleQuery(() => query.GetExpenses(request), log);
        [HttpGet, Route("income")]
        public IActionResult Get([FromQuery] QueryModels.GetIncome request)
            => RequestHandler.HandleQuery(() => query.GetIncome(request), log);
        [HttpGet, Route("outgo")]
        public IActionResult Get([FromQuery] QueryModels.GetOutgo request)
            => RequestHandler.HandleQuery(() => query.GetOutgo(request), log);
        [HttpGet, Route("expense")]
        public IActionResult Get([FromQuery] QueryModels.GetExpense request)
            => RequestHandler.HandleQuery(() => query.GetExpense(request), log);
        [HttpGet, Route("period")]
        public IActionResult Get([FromQuery] QueryModels.GetPeriod request)
            => RequestHandler.HandleQuery(()=> query.GetPeriod(request), log);
    }
}
