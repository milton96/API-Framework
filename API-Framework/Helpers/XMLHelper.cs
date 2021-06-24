using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace API_Framework.Helpers
{
    /// <summary>
    /// Helper para leer datos de un XML
    /// </summary>
    public class XMLHelper
    {
        private string a;
        /// <summary>
        /// Lee un archivo XML
        /// </summary>
        /// <param name="filename">Nombre del archivo</param>
        public XMLHelper(string filename)
        {
            a = String.Format(@"{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, @"Config\", filename);
        }

        /// <summary>
        /// Obtiene el valor de una etiqueta xml
        /// </summary>
        /// <param name="tag">Nombre de la etiqueta</param>
        /// <returns>Valor de la etiqueta</returns>
        public string getValor(string tag)
        {
            string value = "";
            using (var xml = XmlReader.Create(a))
            {
                xml.ReadToFollowing(tag);
                value = xml.ReadElementContentAsString();
            }
            return value;
        }
    }
}