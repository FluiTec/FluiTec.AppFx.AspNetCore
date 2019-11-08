using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>   A controller for handling status codes. </summary>
    public class StatusCodeController : Controller
    {
        /// <summary>	The logger. </summary>
        private readonly ILogger<StatusCodeController> _logger;

        /// <summary>	Constructor. </summary>
        /// <param name="logger">	The logger. </param>
        public StatusCodeController(ILogger<StatusCodeController> logger)
        {
            _logger = logger;
        }

        /// <summary>	Indexes. </summary>
        /// <param name="statusCode">	The status code. </param>
        /// <returns>	An IActionResult. </returns>
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            _logger.LogInformation($"Status Code: {statusCode}, OriginalPath: {reExecute.OriginalPath}");
            return View(statusCode);
        }
    }
}