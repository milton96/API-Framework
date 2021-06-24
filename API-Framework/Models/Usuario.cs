using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public Rol IdRol { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
    }
}