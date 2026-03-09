# Technology Stack

## Framework & Runtime
- .NET Framework 4.8
- ASP.NET MVC 5.2.3
- C# 6.0 language features
- Razor view engine 3.2.3

## Key Libraries
- Microsoft.ApplicationInsights 2.0.0 (telemetry and monitoring)
- Microsoft.Web.Infrastructure
- System.Web.Mvc

## Build System
- MSBuild (ToolsVersion 12.0)
- NuGet package management
- Roslyn compiler platform (Microsoft.Net.Compilers 1.0.0)

## Development Server
- IIS Express
- Default URL: http://localhost:16743/

## Common Commands

Build the solution:
```bash
msbuild WebApplication1/WebApplication1.sln /p:Configuration=Debug
```

Restore NuGet packages:
```bash
nuget restore WebApplication1/WebApplication1.sln
```

Clean build artifacts:
```bash
msbuild WebApplication1/WebApplication1.sln /t:Clean
```

## Configuration
- Debug configuration outputs to `bin/` with full debug symbols
- Release configuration uses optimized compilation with pdb-only debug info
- Web.config transformations available for Debug and Release builds
