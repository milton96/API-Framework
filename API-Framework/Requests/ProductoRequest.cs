using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_Framework.Requests
{
    public class ProductoRequest
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El precio del producto es requerido")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El código del producto es requerido")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "La cantidad de productos es requerida")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "La imagen del producto es requerida")]
        public string Imagen { get; set; }
        public bool Activo { get; set; }
    }
}