using API_Framework.Handlers;
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
    [RoutePrefix("api/producto")]
    public class _ProductoController : ApiController
    {
        [HttpPost]
        [Route("")]
        [Permisos((int)CodigosPermisos.Administrador, (int)CodigosPermisos.Empleado)]
        public async Task<IHttpActionResult> Crear(ProductoRequest producto)
        {
            ErrorHelper errors = new ErrorHelper();
            try
            {
                if (producto == null) throw new Exception("No se recibieron datos");
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

                int usuario_sesion = User.Identity.GetId();

                Producto p = new Producto();
                p.Nombre = producto.Nombre;
                p.Precio = producto.Precio;
                p.Codigo = producto.Codigo;
                p.Stock = producto.Stock;
                p.Imagen = producto.Imagen;
                p.Activo = producto.Activo;
                p.CreadoPor = new Usuario()
                {
                    Id = usuario_sesion
                };
                p.ModificadoPor = new Usuario()
                {
                    Id = usuario_sesion
                };

                p.Id = await p.Guardar();

                return Ok(p);
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
                List<Producto> productos = await Producto.ObtenerTodos();
                return Ok(productos);
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
                if (id <= 0) throw new Exception("No es un producto válido.");
                Producto producto = await Producto.ObtenerPorId(id);
                if (producto == null) throw new Exception("El producto no existe.");
                return Ok(producto);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return Content(HttpStatusCode.BadRequest, errors.GetErrors());
            }
        }
    }
}
