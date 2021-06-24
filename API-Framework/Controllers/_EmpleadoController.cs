using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_Framework.Controllers
{
    [Authorize]
    [RoutePrefix("api/empleado")]
    public class _EmpleadoController : ApiController
    {
        [HttpPost]
        [Route("ping")]
        public async Task<IHttpActionResult> Ping()
        {
            try
            {
                var a = User;
                object res = new
                {
                    estatus = "En sesión"
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
