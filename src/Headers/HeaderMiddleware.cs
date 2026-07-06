using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Webwonders.Baseline.Middleware.Headers;

public static class HeaderMiddleware
{
    /// <summary>Registers header services (HSTS max-age, Server header suppression). Call before <c>Build()</c>.</summary>
    public static WebApplicationBuilder ConfigureHeaders(this WebApplicationBuilder builder)
    {
        var configSection = builder.Configuration.GetSection("Webwonders:Middleware");

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        var maxAgeDays = int.TryParse(configSection[Constants.Headers.HstsMaxAgeDays], out var days)
            ? days
            : Constants.Headers.HstsMaxAgeDaysDefault;
        builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(maxAgeDays));

        return builder;
    }

    /// <summary>Adds the security headers and HSTS to the pipeline. Production only. Call after <c>Build()</c>.</summary>
    public static WebApplication ConfigureHeaders(this WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsProduction())
        {
            var configSection = configuration.GetSection("Webwonders:Middleware");

            if ((configSection[Constants.Headers.Hsts] ?? "true") == "true")
            {
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var xContentTypeOption = configSection[Constants.Headers.XContentTypeOptions]
                    ?? Constants.Headers.XContentTypeOptionsValues.Nosniff;
                context.Response.Headers.Append("X-Content-Type-Options", xContentTypeOption);

                var xFrameOption = configSection[Constants.Headers.XFrameOptions]
                    ?? Constants.Headers.XFrameOptionsValues.SameOrigin;
                context.Response.Headers.Append("X-Frame-Options", xFrameOption);

                var referrerPolicy = configSection[Constants.Headers.ReferrerPolicy]
                    ?? Constants.Headers.ReferrerPolicyValues.StrictOriginWhenCrossOrigin;
                context.Response.Headers.Append("Referrer-Policy", referrerPolicy);

                await next();
            });
        }
        return app;
    }
}