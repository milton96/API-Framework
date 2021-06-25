using API_Framework.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace API_Framework.Handlers
{
    internal class TokenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode status;
            if (!TryGetToken(request, out string token))
            {
                status = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                XMLHelper xml = new XMLHelper("api_config.xml");
                string secret = xml.getValor("secret");
                string issuer = xml.getValor("issuer");
                string audience = xml.getValor("audience");

                SymmetricSecurityKey _ssk = new SymmetricSecurityKey(
                    System.Text.Encoding.Default.GetBytes(secret));

                SecurityToken _st = null;
                JwtSecurityTokenHandler _th = new JwtSecurityTokenHandler();
                TokenValidationParameters _tvp = new TokenValidationParameters()
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifeTimeValidator,
                    IssuerSigningKey = _ssk
                };

                Thread.CurrentPrincipal = _th.ValidateToken(token, _tvp, out _st);
                HttpContext.Current.User = _th.ValidateToken(token, _tvp, out _st);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                status = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() =>
                        new HttpResponseMessage(status) { });
        }

        private static bool TryGetToken(HttpRequestMessage request, out string token)
        {
            token = null;
            bool ok = false;
            if (!request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders) || authHeaders.Count() > 1)
            {
                return ok;
            }

            string[] header = authHeaders.First().Split(' ');
            if (header.Length == 1)
                return ok;

            if (!header.First().ToLower().Equals("bearer"))
                return ok;

            token = header.Last();
            ok = true;

            return ok;
        }

        public bool LifeTimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            bool ok = false;
            if ((expires.HasValue && DateTime.UtcNow < expires)
                && (notBefore.HasValue && DateTime.UtcNow > notBefore))
            {
                ok = true;
            }
            return ok;
        }
    }
}