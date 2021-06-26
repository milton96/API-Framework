using API_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API_Framework.Models
{
    public class Producto : Conexion
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

        public async Task<int> Guardar()
        {
            int id = 0;
            try
            {
                string query = @"INSERT INTO [dbo].[Producto] 
                                VALUES(@Nombre, 
                                        @Precio, 
                                        @Codigo, 
                                        @Stock, 
                                        @Imagen, 
                                        @CreadoPor, 
                                        @ModificadoPor, 
                                        @Creado, 
                                        @Modificado, 
                                        @Activo); 
                                SELECT SCOPE_IDENTITY()";
                using (SqlConnection con = Conectar())
                {
                    using (SqlCommand command = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    })
                    {
                        command.Parameters.AddWithValue("@Nombre", Nombre);
                        command.Parameters.AddWithValue("@Precio", Precio);
                        command.Parameters.AddWithValue("@Codigo", Codigo);
                        command.Parameters.AddWithValue("@Stock", Stock);
                        command.Parameters.AddWithValue("@Imagen", Imagen);
                        command.Parameters.AddWithValue("@CreadoPor", CreadoPor.Id);
                        command.Parameters.AddWithValue("@ModificadoPor", ModificadoPor.Id);
                        command.Parameters.AddWithValue("@Creado", DateTime.Now.ToUTC());
                        command.Parameters.AddWithValue("@Modificado", DateTime.Now.ToUTC());
                        command.Parameters.AddWithValue("@Activo", Activo);
                        con.Open();
                        id = Convert.ToInt32(await command.ExecuteScalarAsync());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }
    }
}