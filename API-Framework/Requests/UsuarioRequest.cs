using API_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_Framework.Requests
{
    public class UsuarioRequest
    {
        [Required(ErrorMessage = "El rol del usuario es requerido")]
        public int Rol { get; set; }
        [Required(ErrorMessage = "El nombre del usuario es requerido")]
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "El correo del usuario es requerido")]
        [RegularExpression(RegexHelper.Correo, ErrorMessage = "El correo no tiene el formato correcto")]
        public string Correo { get; set; }
        //[Required(ErrorMessage = "La contraseña del usuario es requerida")]
        public string Password { get; set; }
    }
}