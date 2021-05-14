using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Labs.NET.AuthenticationHandler.Json
{
    public static class JsonAuthenticationExtensions
    {
        public static AuthenticationBuilder AddJson<TBody>(this AuthenticationBuilder builder, Action<JsonAuthenticationOptions<TBody>> configure)
        {
            builder.Services
                .Configure(configure);

            return builder.AddScheme<JsonAuthenticationOptions<TBody>, JsonAuthenticationHandler<TBody>>(JsonAuthenticationDefaults.Scheme, configure);
        }

        public static AuthenticationBuilder AddJson<TBody>(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<JsonAuthenticationOptions<TBody>, JsonAuthenticationHandler<TBody>>(JsonAuthenticationDefaults.Scheme,
                o => { });
        }
    }
}
