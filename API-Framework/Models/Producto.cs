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

        public static async Task<List<Producto>> ObtenerTodos()
        {
            List<Producto> productos = new List<Producto>();
            try
            {
                string query = @"SELECT * FROM [dbo].[Producto]";
                using (SqlConnection con = Conectar())
                {
                    using (SqlCommand command = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    })
                    {
                        con.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var i = new
                            {
                                Id = reader.GetOrdinal("Id"),
                                CreadoPor = reader.GetOrdinal("CreadoPor"),
                                ModificadoPor = reader.GetOrdinal("ModificadoPor"),
                                Nombre = reader.GetOrdinal("Nombre"),
                                Precio = reader.GetOrdinal("Precio"),
                                Codigo = reader.GetOrdinal("Codigo"),
                                Stock = reader.GetOrdinal("Stock"),
                                Imagen = reader.GetOrdinal("Imagen"),
                                Creado = reader.GetOrdinal("Creado"),
                                Modificado = reader.GetOrdinal("Modificado"),
                                Activo = reader.GetOrdinal("Activo")
                            };

                            while(reader.Read())
                            {
                                Producto p = new Producto();
                                p.Id = reader.GetValor<int>(i.Id);
                                p.CreadoPor = await Usuario.ObtenerPorId(reader.GetValor<int>(i.CreadoPor));
                                p.ModificadoPor = await Usuario.ObtenerPorId(reader.GetValor<int>(i.ModificadoPor));
                                p.Nombre = reader.GetValor<string>(i.Nombre);
                                p.Precio = reader.GetValor<decimal>(i.Precio);
                                p.Codigo = reader.GetValor<string>(i.Codigo);
                                p.Stock = reader.GetValor<int>(i.Stock);
                                p.Imagen = reader.GetValor<string>(i.Imagen);
                                p.Creado = reader.GetValor<DateTime>(i.Creado).ToCST();
                                p.Modificado = reader.GetValor<DateTime>(i.Modificado).ToCST();
                                productos.Add(p);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return productos;
        }
    }
}