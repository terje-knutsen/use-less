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
        private readonly IQueryService<ReadModels.Budget> query;
        private static readonly ILogger log = Log.ForContext<ReadModels.Budget>();
        public BudgetQueryApi(IQueryService<ReadModels.Budget> query)
        {
            this.query = query;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] QueryModels.GetBudget request)
        => RequestHandler.HandleQuery(() => query.Query(request),log);
    }
}
