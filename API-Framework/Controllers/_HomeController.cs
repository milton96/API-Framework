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
using System.Web.Http.ModelBinding;

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

                if (!ModelState.IsValid)
                {
                    foreach (KeyValuePair<string, ModelState> model in ModelState)
                    {
                        foreach (ModelError error in model.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }

                    return Content(HttpStatusCode.BadRequest, errors.GetErrors());
                }

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

        [HttpPost]
        [Route("tabla-prueba")]
        public IHttpActionResult TablaPrueba()
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                TablaHelper tabla = TablaHelper.TablaPrueba(5, 5);

                tabla.Ordenar();
                object res = new
                {
                    tabla
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                object res = new
                {
                    errors = errors.GetErrors(),
                    tabla = new TablaHelper()
                };
                return Content(HttpStatusCode.BadRequest, res);
            }
        }
    }
}
