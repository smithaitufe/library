using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Web.Controllers {
    [AllowAnonymous]
    public class ErrorController: Controller {

        private readonly ILogger<ErrorController> _logger;
        
        public ErrorController(ILogger<ErrorController> logger) {
            _logger = logger;
        }
        [Route("/Error/{statusCode}")]
        public IActionResult Index (int statusCode){
            var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            _logger.LogInformation($"Unexpected Status Code: {statusCode}, Original Path: {reExecute.OriginalPath}");
            return View(statusCode);
        }
    }
}