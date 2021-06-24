using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Codigo { get; set; }
        public bool Activo { get; set; }
    }
}