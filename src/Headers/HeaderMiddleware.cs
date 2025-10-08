using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Webwonders.Baseline.Middleware.Headers;

public static class HeaderMiddleware
{
    public static WebApplication ConfigureHeaders(this WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsProduction())
        {
            var configSection = configuration.GetSection("Webwonders:Middleware");

            app.Use(async (context, next) =>
            {
                var xContentTypeOption = configSection[Constants.Headers.XContentTypeOptions] 
                    ?? Constants.Headers.XContentTypeOptionsValues.Nosniff;
                context.Response.Headers.Append("X-Content-Type-Options", xContentTypeOption);

                var xFrameOption = configSection[Constants.Headers.XFrameOptions] 
                    ?? Constants.Headers.XFrameOptionsValues.SameOrigin;
                context.Response.Headers.Append("X-Frame-Options", xFrameOption);

                var xssProtection = configSection[Constants.Headers.XxssProtection] 
                    ?? Constants.Headers.XxssProtectionValues.Enabled;
                context.Response.Headers.Append("X-XSS-Protection", xssProtection);

                if ((configSection[Constants.Headers.Hsts] ?? "true") == "true")
                {
                    app.UseHsts();
                }

                await next();
            });
        }
        return app;
    }
}