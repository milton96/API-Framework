using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Framework.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class _HomeController : ApiController
    {
        [HttpPost]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("API Up");
        }
    }
}
