using API_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
                            Stopwatch st = new Stopwatch();
                            st.Start();
                            while(reader.Read())
                            {
                                Task<Usuario> creadopor = Usuario.ObtenerPorId(reader.GetValor<int>(i.CreadoPor));
                                Task<Usuario> modificadopor = Usuario.ObtenerPorId(reader.GetValor<int>(i.ModificadoPor));
                                Producto p = new Producto();
                                p.Id = reader.GetValor<int>(i.Id);
                                //p.CreadoPor = await Usuario.ObtenerPorId(reader.GetValor<int>(i.CreadoPor));
                                //p.ModificadoPor = await Usuario.ObtenerPorId(reader.GetValor<int>(i.ModificadoPor));
                                p.Nombre = reader.GetValor<string>(i.Nombre);
                                p.Precio = reader.GetValor<decimal>(i.Precio);
                                p.Codigo = reader.GetValor<string>(i.Codigo);
                                p.Stock = reader.GetValor<int>(i.Stock);
                                p.Imagen = reader.GetValor<string>(i.Imagen);
                                p.Creado = reader.GetValor<DateTime>(i.Creado).ToCST();
                                p.Modificado = reader.GetValor<DateTime>(i.Modificado).ToCST();
                                
                                await Task.WhenAll(creadopor, modificadopor);

                                p.CreadoPor = creadopor.Result;
                                p.ModificadoPor = modificadopor.Result;
                                productos.Add(p);
                            }
                            st.Stop();
                            Debug.WriteLine($"Tiempo invertido con Task.WhenAll: {st.Elapsed.TotalMilliseconds}");
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

        public static async Task<Producto> ObtenerPorId(int id)
        {
            Producto producto = null;
            try
            {
                string query = @"SELECT * FROM [dbo].[Producto] WHERE Id = @Id";
                using (SqlConnection con = Conectar())
                {
                    using (SqlCommand command = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    })
                    {
                        command.Parameters.AddWithValue("@Id", id);
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

                            if (reader.Read())
                            {
                                Task<Usuario> creadopor = Usuario.ObtenerPorId(reader.GetValor<int>(i.CreadoPor));
                                Task<Usuario> modificadopor = Usuario.ObtenerPorId(reader.GetValor<int>(i.ModificadoPor));
                                producto = new Producto();
                                producto.Id = reader.GetValor<int>(i.Id);
                                producto.Nombre = reader.GetValor<string>(i.Nombre);
                                producto.Precio = reader.GetValor<decimal>(i.Precio);
                                producto.Codigo = reader.GetValor<string>(i.Codigo);
                                producto.Stock = reader.GetValor<int>(i.Stock);
                                producto.Imagen = reader.GetValor<string>(i.Imagen);
                                producto.Creado = reader.GetValor<DateTime>(i.Creado).ToCST();
                                producto.Modificado = reader.GetValor<DateTime>(i.Modificado).ToCST();

                                await Task.WhenAll(creadopor, modificadopor);

                                producto.CreadoPor = creadopor.Result;
                                producto.ModificadoPor = modificadopor.Result;
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
            return producto;
        }
    }
}