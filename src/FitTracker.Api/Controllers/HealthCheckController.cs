using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitTracker.Api.Controllers
{
    [Route("/")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
