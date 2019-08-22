using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.Core.Api.Controllers
{
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet(""),HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}