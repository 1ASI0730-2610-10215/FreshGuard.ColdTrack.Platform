# FreshGuard ColdTrack Platform

FreshGuard ColdTrack Platform is an ASP.NET Core RESTful API for cold-chain shipment monitoring. It supports identity and access management, shipment management, telemetry monitoring, alerting, analytics, and PDF reporting.

## Bounded Contexts

- `Iam`: user registration, authentication, JWT tokens, and roles.
- `ShipmentManagement`: refrigerated shipments and lifecycle transitions.
- `TelemetryMonitoring`: sensors, sensor assignments, and temperature/humidity readings.
- `Alerting`: threshold evaluation and alert lifecycle.
- `Analytics`: dashboard indicators, shipment history, and PDF reports.
- `Shared`: shared kernel for persistence, error handling, localization, and REST conventions.

## Requirements

- .NET SDK 10.
- MySQL 8.x.
- Entity Framework Core CLI.

## Local Development

Configure `FreshGuard.ColdTrack.Platform/appsettings.Development.json` or override the connection string with an environment variable.

```powershell
dotnet restore
dotnet build cold-track-platform.sln
dotnet test cold-track-platform.sln
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run --project FreshGuard.ColdTrack.Platform
```

Swagger is available in development at the URL printed by ASP.NET Core, usually `/swagger`.

Demo credentials are created when `Database:SeedDemoData` is enabled:

```text
admin@coldtrack.local / Password123!
driver@coldtrack.local / Password123!
```

## Render And Filess.io Deployment

Create a Docker Web Service in Render and configure the environment variables manually in the Render dashboard. Do not commit secrets to the repository.

```text
ASPNETCORE_ENVIRONMENT=Production
DATABASE_HOST=xx0uzg.h.filess.io
DATABASE_PORT=3307
DATABASE_NAME=coldtrack_platform_centerill
DATABASE_USER=coldtrack_platform_centerill
DATABASE_PASSWORD=<Filess.io password>
TokenSettings__Secret=<long secure secret>
TokenSettings__Issuer=FreshGuard.ColdTrack.Platform
TokenSettings__Audience=ColdTrack.WebApplication
```

The production connection string is defined with placeholders in `appsettings.Production.json` and requires SSL:

```text
server=%DATABASE_HOST%;port=%DATABASE_PORT%;database=%DATABASE_NAME%;user=%DATABASE_USER%;password=%DATABASE_PASSWORD%;SslMode=Required
```

For the Filess.io instance prepared for this project, only copy the password directly from the Filess.io dashboard into `DATABASE_PASSWORD`. Keep JWT values in Render variables using the `TokenSettings__` prefix because ASP.NET Core maps double underscores to nested configuration keys.

## Production Endpoints

- `GET /health`
- `POST /api/v1/authentication/sign-up`
- `POST /api/v1/authentication/sign-in`
- `GET /api/v1/users/me`
- `POST /api/v1/shipments`
- `GET /api/v1/shipments`
- `PATCH /api/v1/shipments/{shipmentId}/status`
- `POST /api/v1/sensors`
- `GET /api/v1/sensors`
- `PATCH /api/v1/sensors/{sensorId}/assignment`
- `POST /api/v1/telemetry`
- `GET /api/v1/shipments/{shipmentId}/telemetry`
- `GET /api/v1/alerts`
- `PATCH /api/v1/alerts/{alertId}/acknowledgment`
- `PATCH /api/v1/alerts/{alertId}/resolution`
- `GET /api/v1/analytics/dashboard`
- `GET /api/v1/analytics/shipment-history`
- `POST /api/v1/reports`
- `GET /api/v1/reports`
- `GET /api/v1/reports/{reportId}/file`

Swagger is enabled in production through `Swagger:Enabled=true` in `appsettings.Production.json`.
