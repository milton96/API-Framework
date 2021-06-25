using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Codigo { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; }
        public Usuario CreadoPor { get; set; }
        public Usuario ModificadoPor { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Modificado { get; set; }
        public bool Activo { get; set; }
    }
}