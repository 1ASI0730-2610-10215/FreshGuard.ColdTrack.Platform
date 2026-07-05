# FreshGuard ColdTrack Platform

FreshGuard ColdTrack Platform is an ASP.NET Core RESTful API for cold-chain shipment monitoring. It supports identity and access management, shipment management, telemetry monitoring, alerting, analytics, and PDF reporting.

## Bounded Contexts

- `Iam`: user registration, authentication, JWT tokens, and roles.
- `ShipmentManagement`: refrigerated shipments and lifecycle transitions.
- `TelemetryMonitoring`: sensors, sensor assignments, and temperature/humidity readings.
- `Alerting`: threshold evaluation and alert lifecycle.
- `Analytics`: dashboard indicators, shipment history, and PDF reports.
- `Shared`: shared kernel for persistence, error handling, localization, and REST conventions.

## Architecture Guidelines

The API follows a Domain-Driven Design structure aligned with the course reference projects. Each bounded context keeps its business rules inside the domain layer, exposes use cases through application services, and isolates technical integrations in infrastructure. Controllers and REST resources remain in the interfaces layer so the HTTP API does not leak persistence concerns into the domain model.

The current implementation uses these patterns:

- **Domain Model**: entities, value objects, repositories, services, and commands represent the cold-chain business language.
- **Application Services**: command and query services coordinate use cases without owning infrastructure details.
- **Infrastructure**: Entity Framework Core repositories, MySQL persistence, JWT token generation, BCrypt hashing, and QuestPDF report generation.
- **Interfaces**: REST controllers, request resources, response resources, assemblers, OpenAPI annotations, and API versioned routes.
- **Shared Kernel**: common persistence contracts, unit of work, localization resources, global error handling, and route conventions.

## Layer Responsibilities

- `Domain`: owns business invariants such as shipment lifecycle transitions, telemetry thresholds, alert states, and report metadata.
- `Application`: orchestrates commands and queries, validates use-case flow, and coordinates between repositories and domain services.
- `Infrastructure`: implements persistence, security, hashing, token creation, PDF rendering, and database initialization.
- `Interfaces`: exposes REST endpoints under `/api/v1`, maps resources to domain commands, and returns API-friendly responses.
- `Resources`: stores localized messages used by validation and problem detail responses.

## Frontend Integration

The deployed Vue frontend is allowed to call the API through the configured CORS policy. Production currently accepts:

- `https://coldtrack-front-web.web.app`
- `https://coldtrack-front-web.firebaseapp.com`

Development accepts:

- `http://localhost:5173`
- `http://127.0.0.1:5173`

If a new frontend domain is added, update `Cors:AllowedOrigins` in the corresponding `appsettings` file or configure the same key through Render environment variables.

## Security And API Access

Authentication uses JWT Bearer tokens. Clients must sign in through `POST /api/v1/authentication/sign-in`, copy the returned token, and send it in the `Authorization` header as `Bearer <token>`.

Protected endpoints include shipment management, telemetry, sensors, alerts, analytics, reports, and the authenticated user profile. Swagger is enabled in production to support Sprint Review demonstrations, but it still requires authorization for protected endpoints.

## Requirements

- .NET SDK 10.
- MySQL 8.x.
- Entity Framework Core CLI.

## Local Development

Configure `FreshGuard.ColdTrack.Platform/appsettings.Development.json` or create a local `FreshGuard-ColdTrack-Platform.env` file from `FreshGuard-ColdTrack-Platform.env.example` to override the production-ready environment variables. The local environment file is ignored by Git because it may contain secrets.

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
server=%DATABASE_HOST%;port=%DATABASE_PORT%;database=%DATABASE_NAME%;user=%DATABASE_USER%;password=%DATABASE_PASSWORD%;SslMode=Required;Allow User Variables=True
```

For the Filess.io instance prepared for this project, only copy the password directly from the Filess.io dashboard into `DATABASE_PASSWORD`. Keep JWT values in Render variables using the `TokenSettings__` prefix because ASP.NET Core maps double underscores to nested configuration keys.

## Production Infrastructure

- **Render** hosts the ASP.NET Core API as a Docker Web Service.
- **Filess.io** hosts the MySQL 8 database using SSL.
- **Swagger/OpenAPI** documents and validates the available REST endpoints.
- **QuestPDF** generates PDF reports from analytics data.
- **Entity Framework Core** applies migrations and seeds demonstration data when enabled.

The API expands environment placeholders at startup before connecting to MySQL. This keeps deployment secrets outside Git while preserving a reproducible production configuration.

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

## Verification Checklist

Run the following commands before merging a feature branch:

```powershell
dotnet restore cold-track-platform.sln
dotnet build cold-track-platform.sln
dotnet test cold-track-platform.sln
git diff --check
git status --short
```

For a functional smoke test:

1. Start the API locally or open the Render Swagger URL.
2. Call `POST /api/v1/authentication/sign-in` with the demo administrator credentials.
3. Authorize Swagger with the returned JWT token.
4. Execute `GET /api/v1/shipments`, `GET /api/v1/sensors`, `GET /api/v1/alerts`, and `GET /api/v1/analytics/dashboard`.
5. Register a telemetry reading through `POST /api/v1/telemetry` and verify that related shipment, sensor, and alert data are updated.
6. Generate a report with `POST /api/v1/reports` and download it with `GET /api/v1/reports/{reportId}/file`.
