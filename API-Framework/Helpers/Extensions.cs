using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace API_Framework.Helpers
{
    public static class Extensions
    {

        public static T GetValor<T>(this SqlDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? default : (T)reader.GetValue(index);
        }

        public static object ToDB(this String value)
        {
            return String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value) ? DBNull.Value : (object)value;
        }

        public static object ToDB(this int value)
        {
            return value <= 0 ? DBNull.Value : (object)value;
        }

        public static int GetId(this IIdentity user)
        {
            ClaimsIdentity currentUser = user as ClaimsIdentity;
            Claim claim = currentUser.FindFirst(ClaimTypes.NameIdentifier);
            if (!Int32.TryParse(claim.Value, out int id))
                throw new Exception("El usuario no es válido");
            return id;
        }
    }
}