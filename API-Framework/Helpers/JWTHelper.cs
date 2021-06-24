using API_Framework.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace API_Framework.Helpers
{
    public class JWTHelper
    {
        public static async Task<string> GenerarToken(Usuario usuario)
        {
            XMLHelper xml = new XMLHelper("api_config.xml");
            string secret = xml.getValor("secret");
            string issuer = xml.getValor("issuer");
            string audience = xml.getValor("audience");
            if (!Int32.TryParse(xml.getValor("expires"), out int expires))
                expires = 5;

            // header
            SymmetricSecurityKey _ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials _sc = new SigningCredentials(_ssk, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new JwtHeader(_sc);

            // claims
            Claim[] _claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.IdRol.Codigo.ToString())
            };

            // payload
            JwtPayload _payload = new JwtPayload(issuer, audience, _claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expires));

            // token
            JwtSecurityToken _token = new JwtSecurityToken(header, _payload);

            return new JwtSecurityTokenHandler().WriteToken(_token);            
        }
    }
}