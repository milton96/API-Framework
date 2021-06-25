using API_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API_Framework.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermisosAttribute : ActionFilterAttribute
    {
        private int[] _roles;
        public PermisosAttribute(params int[] roles) 
        {
            _roles = roles;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                IIdentity identity = actionContext.RequestContext.Principal.Identity;
                ClaimsIdentity user = identity as ClaimsIdentity;
                Claim permiso_rol = user.FindFirst(ClaimTypes.Role);
                int rol = Int32.Parse(permiso_rol.Value);
                if (!_roles.Contains(rol))
                {
                    ErrorHelper error = new ErrorHelper();
                    error.Add("No se cuentan con los permisos necesarios");
                    HttpResponseMessage res = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, error.GetErrors());
                    actionContext.Response = res;
                }
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                HttpResponseMessage res = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, "Ha ocurrido algo inesperado al verificar los permisos");
                actionContext.Response = res;
            }
        }        
    }
}