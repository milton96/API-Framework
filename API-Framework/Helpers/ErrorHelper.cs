using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Helpers
{
    public class ErrorHelper
    {
        private List<string> Errors { get; set; }
        public bool HasErrors 
        { 
            get
            {
                return Errors.Any();
            } 
        }

        public ErrorHelper()
        {
            Errors = new List<string>();
        }

        public void Add(string mensaje)
        {
            Errors.Add(mensaje);
        }

        public List<string> GetErrors()
        {
            return Errors;
        }
    }
}