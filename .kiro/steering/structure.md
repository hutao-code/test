# Project Structure

## Solution Organization
```
WebApplication1/
├── WebApplication1.sln          # Solution file
└── WebApplication1/             # Main web project
    ├── App_Start/               # Application startup configuration
    │   └── RouteConfig.cs       # MVC routing rules
    ├── Controllers/             # MVC controllers
    │   └── DefaultController.cs
    ├── Models/                  # Data models (empty, ready for use)
    ├── Views/                   # Razor views
    │   ├── Shared/              # Shared layouts and partials
    │   │   └── _Layout.cshtml
    │   └── web.config           # View engine configuration
    ├── scripts/                 # JavaScript files
    │   ├── ai.*.js              # Application Insights scripts
    │   ├── mobileCompat.js
    │   └── test.js
    ├── Properties/              # Assembly metadata
    ├── Global.asax(.cs)         # Application lifecycle events
    ├── Web.config               # Application configuration
    └── ApplicationInsights.config
```

## Architecture Patterns

### MVC Pattern
- Controllers handle HTTP requests and return ActionResults
- Views use Razor syntax (.cshtml files)
- Models folder prepared for domain/view models

### Routing
- Convention-based routing: `{controller}/{action}/{id}`
- Default route: Home/Index
- Routes configured in `App_Start/RouteConfig.cs`

### Application Startup
- `Global.asax.cs` handles application lifecycle
- Area registration and route registration occur in `Application_Start()`

## Naming Conventions
- Controllers: `{Name}Controller.cs` (e.g., DefaultController)
- Actions: PascalCase method names
- Views: Match controller/action names
- Namespace: `WebApplication1` (root), `WebApplication1.Controllers`, etc.

## Configuration Files
- `Web.config`: Main application settings, connection strings, compilation settings
- `Web.Debug.config` / `Web.Release.config`: Environment-specific transforms
- `ApplicationInsights.config`: Telemetry configuration
- `packages.config`: NuGet package references
