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

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Crear(UsuarioRequest usuario)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (usuario == null) throw new Exception("No se recibieron datos");

                if (String.IsNullOrEmpty(usuario.Password) || String.IsNullOrWhiteSpace(usuario.Password))
                {
                    ModelState.AddModelError("Password", "La contraseña de usuario es requerida");
                }

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

                Rol rol = await Rol.ObtenerPorCodigo(usuario.Rol);
                if (rol == null) throw new Exception("El rol no es válido");                

                Usuario u = new Usuario();
                u.IdRol = rol;
                u.Nombre = usuario.Nombre;
                u.ApellidoPaterno = usuario.ApellidoPaterno;
                u.ApellidoMaterno = usuario.ApellidoMaterno;
                u.Correo = usuario.Correo;
                u.Password = usuario.Password;
                u.Id = await u.Guardar();
                return Ok(u);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Obtener()
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                List<Usuario> usuarios = await Usuario.ObtenerTodos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Obtener(int id)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (id <= 0) throw new Exception("Usuario no válido");
                Usuario usuario = await Usuario.ObtenerPorId(id);
                if (usuario == null) throw new Exception("Usuario inexistente");
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Desactivar(int id)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (id <= 0) throw new Exception("Usuario no válido");
                Usuario u = await Usuario.ObtenerPorId(id);
                if (u == null) throw new Exception("Usuario inexistente");

                int total = await u.Desactivar();
                if (total <= 0) throw new Exception("Ocurrió un problema al desactivar el usuario");

                return Ok();
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Editar(int id, UsuarioRequest usuario)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (id <= 0 || usuario == null) throw new Exception("Usuario no válido");

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

                Usuario u = await Usuario.ObtenerPorId(id);
                if (u == null) throw new Exception("Usuario inexistente");

                if (usuario.Rol > 0)
                {
                    Rol rol = await Rol.ObtenerPorCodigo(usuario.Rol);
                    if (rol == null) throw new Exception("El rol del usuario es inválido");
                    if (rol.Id != u.IdRol.Id)
                        u.IdRol = rol;
                }

                u.Nombre = usuario.Nombre;
                u.ApellidoPaterno = usuario.ApellidoPaterno;
                u.ApellidoMaterno = usuario.ApellidoMaterno;

                await u.Actualizar();

                return Ok("Usuario actualizado");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }
    }
}
