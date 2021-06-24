using API_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API_Framework.Models
{
    public class Conexion
    {
        public static SqlConnection Conectar()
        {
            SqlConnection con = null;
            try
            {
                string query = GetConnectionString();
                con = new SqlConnection(query);
            }
            catch (Exception ex)
            {
                con = null;
            }
            return con;
        }

        public static async Task<bool> TestConexion()
        {
            bool ok = false;
            try
            {
                string query = "select 1";
                using (SqlConnection con = Conectar())
                {
                    using (SqlCommand command = new SqlCommand(query, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                        CommandTimeout = 60
                    })
                    {
                        con.Open();                        
                        System.Diagnostics.Debug.WriteLine("Conexión exitosa");

                        System.Diagnostics.Debug.WriteLine("Query ok");
                        await command.ExecuteNonQueryAsync();
                        
                        ok = true;
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ok = false;
            }
            return ok;
        }

        private static string GetConnectionString()
        {            
            string query = "";
            try
            {
                XMLHelper xml = new XMLHelper("db_config.xml");
                string server = "", database = "", user = "", pass = "", port = "";
                string template = @"Server={0}{1};Database={2};User Id={3};Password={4};";

                server = xml.getValor("server");
                port = xml.getValor("port");
                database = xml.getValor("database");
                user = xml.getValor("user");
                pass = xml.getValor("pass");

                if (!port.Equals(String.Empty))
                {
                    template = @"Server={0},{1};Database={2};User Id={3};Password={4};";
                }

                query = String.Format(template, server, port, database, user, pass);
            }
            catch (Exception ex)
            {
                query = "";
            }
            return query;
        }
    }
}