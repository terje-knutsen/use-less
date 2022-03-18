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
        public async Task<IActionResult> Post(V1.Create request)
        {
            await applicationService.Handle(request);
            return Ok();
        }
    }
}
