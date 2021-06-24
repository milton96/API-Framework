using API_Framework.Helpers;
using API_Framework.Models;
using API_Framework.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_Framework.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class _HomeController : ApiController
    {
        [HttpPost]
        [Route("ping")]
        public async Task<IHttpActionResult> Ping()
        {
            try
            {
                bool db_ok = await Conexion.TestConexion();
                string msj_db = db_ok ? "Conexión exitosa" : "Ocurrió un problema al realizar la conexión.";

                object res = new
                {
                    db = msj_db,
                    estatus = "API en línea"
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginRequest login)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (login == null) throw new Exception("No se proporcionaron datos");

                Usuario usuario = await Usuario.Login(login);

                string token = await JWTHelper.GenerarToken(usuario);

                object res = new
                {
                    token
                };

                return Ok(res);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }
    }
}
