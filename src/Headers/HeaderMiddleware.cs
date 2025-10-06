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
            // Get the setting from appsettings whether this should be on or not
            var configSection = configuration.GetSection("Webwonders:Middleware");

            if (configSection != null)
            {
                app.Use(async (context, next) =>
                {
                    var xContentTypeOption = configSection[Constants.Headers.XContentTypeOptions];
                    if (xContentTypeOption == Constants.Headers.XContentTypeOptionsValues.Nosniff)
                    {
                        context.Response.Headers.Append("X-Content-Type-Options", xContentTypeOption);
                    }

                    var xFrameOption = configSection[Constants.Headers.XFrameOptions];
                    if (xFrameOption == Constants.Headers.XFrameOptionsValues.SameOrigin ||
                        xFrameOption == Constants.Headers.XFrameOptionsValues.Deny ||
                        xFrameOption == Constants.Headers.XFrameOptionsValues.AllowFrom)
                    {
                        context.Response.Headers.Append("X-Frame-Options", xFrameOption);
                    }

                    var xssProtection = configSection[Constants.Headers.XxssProtection];
                    if (xssProtection == Constants.Headers.XxssProtectionValues.Enabled ||
                        xssProtection == Constants.Headers.XxssProtectionValues.Disabled)
                    {
                        context.Response.Headers.Append("X-XSS-Protection", xssProtection);
                    }
                    
                    if (configSection[Constants.Headers.Hsts] == "true")
                    {
                        app.UseHsts();
                    } 
                    
                    await next();
                });
            }
        }
        return app;
    }
}