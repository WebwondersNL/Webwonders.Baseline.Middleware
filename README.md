# Webwonders.Baseline.Middleware

A simple ASP\.NET Core middleware package to automatically add security headers to your application.

## Features

- Adds the following headers by default:
    - `X-Frame-Options: SAMEORIGIN`
    - `X-Content-Type-Options: nosniff`
    - `X-XSS-Protection: 1; mode=block`
- Supports overriding header values via `appsettings.json`
- Optionally enables HSTS

## Installation

Add the NuGet package to your project:

```cmd
  dotnet add package Webwonders.Baseline.Middleware
```

## Usage

1. Add the middleware to your Program.cs

```csharp
using Webwonders.Baseline.Middleware.Headers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.ConfigureHeaders(app.Environment, builder.Configuration);

app.Run();
```

2. (Optional) Configure header values in `appsettings.json`

```json
{
  "Webwonders": {
    "Middleware": {
      "XFrameOptions": "SAMEORIGIN",
      "XContentTypeOptions": "nosniff",
      "XXSSProtection": "1; mode=block",
      "Hsts": {
        "Enabled": true
      },
    },
  },
}
```

## Default Values

| Header               | Default Value         |
|----------------------|-----------------------|
| X-Frame-Options      | SAMEORIGIN            |
| X-Content-Type-Options | nosniff              |
| X-XSS-Protection     | 1; mode=block         |
| HSTS                 | Disabled by default   |

## License
This project is licensed under the MIT License.