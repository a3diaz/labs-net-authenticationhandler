using Microsoft.AspNetCore.Authentication;

namespace Labs.NET.AuthenticationHandler.Json
{
    public class JsonAuthenticationOptions<TBody> : AuthenticationSchemeOptions
    {
        public TBody Body { get; set; }
    }
}
