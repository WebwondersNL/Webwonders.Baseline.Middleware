# Webwonders.Baseline.Middleware

A simple ASP\.NET Core middleware package to automatically add security headers to an Umbraco application, aligned with the Umbraco backoffice **Settings → Health Check → Security** checks.

## Features

- In **Production only**, adds the following response headers (all overridable via `appsettings.json`):
    - `X-Frame-Options: SAMEORIGIN`
    - `X-Content-Type-Options: nosniff`
    - `Referrer-Policy: strict-origin-when-cross-origin`
- Enables **HSTS** (Production only, on by default) with a configurable `max-age` (defaults to 366 days).
- Suppresses the Kestrel `Server` response header (satisfies the *Excessive Headers* health check).
- Does **not** send `X-XSS-Protection`. The header is deprecated (MDN/OWASP) and Umbraco's health check was inverted in 11.3.0 so it now *fails* when the header is present.

## Installation

```cmd
  dotnet add package Webwonders.Baseline.Middleware
```

## Usage

The package has two entry points: one on the **builder** (services / Kestrel config) and one on the **app** (the request pipeline). Both share the name `ConfigureHeaders`.

```csharp
using Webwonders.Baseline.Middleware.Headers;

var builder = WebApplication.CreateBuilder(args);

// 1. Before Build(): configures HSTS max-age and suppresses the Server header.
builder.ConfigureHeaders();

var app = builder.Build();

// 2. After Build(): adds the security headers + HSTS to the pipeline (Production only).
app.ConfigureHeaders(app.Environment, builder.Configuration);

app.Run();
```

Place the pipeline call early, before `UseUmbraco()`. HSTS is registered once at startup (never per request) and only in Production, so it is never sent on `localhost` — this avoids a stale browser HSTS cache breaking local development.

## Configuration (optional)

All values live under `Webwonders:Middleware`. Header values default to the values below when the key is absent.

```json
{
  "Webwonders": {
    "Middleware": {
      "Enable_HSTS": "true",
      "HSTS_MaxAgeDays": "366",
      "Enable_X-Frame-Options": "SAMEORIGIN",
      "Enable_X-ContentType-Options": "nosniff",
      "Enable_Referrer-Policy": "strict-origin-when-cross-origin"
    }
  }
}
```

| Config key | Header | Default |
|----------------------------|--------------------------|-------------------------------|
| `Enable_HSTS`              | `Strict-Transport-Security` (via `UseHsts`) | `true` (enabled) |
| `HSTS_MaxAgeDays`          | HSTS `max-age`           | `366` |
| `Enable_X-Frame-Options`   | `X-Frame-Options`        | `SAMEORIGIN` |
| `Enable_X-ContentType-Options` | `X-Content-Type-Options` | `nosniff` |
| `Enable_Referrer-Policy`   | `Referrer-Policy`        | `strict-origin-when-cross-origin` |

## Removing `X-Powered-By` (IIS / Umbraco Cloud)

The `Server` header is handled by this package, but `X-Powered-By` is added by IIS *after* the ASP.NET pipeline, so it cannot be removed from middleware. To make the *Excessive Headers* health check fully green on IIS hosting, add this to `web.config`:

```xml
<system.webServer>
  <httpProtocol>
    <customHeaders>
      <remove name="X-Powered-By" />
    </customHeaders>
  </httpProtocol>
</system.webServer>
```

## License
This project is licensed under the MIT License.
