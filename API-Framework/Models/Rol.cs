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
    public class Rol : Conexion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Codigo { get; set; }
        public bool Activo { get; set; }

        public static async Task<Rol> ObtenerPorCodigo(int codigo)
        {
            Rol r = null;
            try
            {
                string query = @"SELECT * FROM [dbo].[Rol] R WHERE R.Codigo = @Codigo";
                using (SqlConnection con = Conectar())
                {
                    using (SqlCommand command = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 60
                    })
                    {
                        command.Parameters.AddWithValue("@Codigo", codigo);
                        con.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var i = new
                            {
                                Id = reader.GetOrdinal("Id"),
                                Nombre = reader.GetOrdinal("Nombre"),
                                Codigo = reader.GetOrdinal("Codigo"),
                                Activo = reader.GetOrdinal("Activo")
                            };

                            if (reader.Read())
                            {
                                r = new Rol();
                                r.Id = reader.GetValor<int>(i.Id);
                                r.Nombre = reader.GetValor<string>(i.Nombre);
                                r.Codigo = reader.GetValor<int>(i.Codigo);
                                r.Activo = reader.GetValor<bool>(i.Activo);
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
            return r;
        }
    }
}