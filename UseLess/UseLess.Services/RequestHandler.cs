using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseLess.Services
{
    public class RequestHandler
    {
        public static async Task<IActionResult> HandleQuery<TModel>(Func<Task<TModel>> query, ILogger log)
        {
            try 
            {
                var request = await query();
                return new OkObjectResult(request);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error handling the query");
                return new BadRequestObjectResult(new 
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
