using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Labs.NET.AuthenticationHandler.Json
{
    public class JsonAuthenticationHandler<TBody> : AuthenticationHandler<JsonAuthenticationOptions<TBody>>
    {
        public JsonAuthenticationHandler(IOptionsMonitor<JsonAuthenticationOptions<TBody>> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimsPrincipal = default(ClaimsPrincipal);
            if(Options.Body != null)
            {
                claimsPrincipal = BuildClaims(Options.Body);
            }
            else
            {
                if (!Request.Headers.TryGetValue("Authorization", out var headers))
                    return Task.FromResult(AuthenticateResult.Fail("No authorization header found."));

                var header = headers.FirstOrDefault(h => h.StartsWith(JsonAuthenticationDefaults.Scheme));

                if (string.IsNullOrEmpty(header))
                    return Task.FromResult(AuthenticateResult.Fail($"No {JsonAuthenticationDefaults.Scheme} authorization header found."));

                var segments = header.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length != 2)
                    return Task.FromResult(AuthenticateResult.Fail("Authorization header has an invalid format."));

                var value = segments[1];
                var bytes = Convert.FromBase64String(value);

                var json = Encoding.UTF8.GetString(bytes);
                var body = JsonSerializer.Deserialize<TBody>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                claimsPrincipal = BuildClaims(body);
            }
            
            var auhtTicket = new AuthenticationTicket(claimsPrincipal, JsonAuthenticationDefaults.Scheme);
            return Task.FromResult(AuthenticateResult.Success(auhtTicket));
        }

        private ClaimsPrincipal BuildClaims(TBody body)
        {
            var props = body.GetType()
                            .GetProperties();

            var claims = new List<Claim>();
            foreach (var prop in props)
            {
                claims.Add(new Claim(prop.Name, $"{prop.GetValue(body)}"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, JsonAuthenticationDefaults.Scheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}
