using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Helpers
{
    public class RegexHelper
    {
        public const string Correo = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    }
}