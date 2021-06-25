using API_Framework.Helpers;
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
        
    }
}
