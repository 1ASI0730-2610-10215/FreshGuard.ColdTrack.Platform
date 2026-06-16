using System.Text;
using FreshGuard.ColdTrack.Platform.Analytics.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.Internal.CommandServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.Internal.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.OutboundServices;
using FreshGuard.ColdTrack.Platform.Analytics.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Analytics.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Documents.QuestPdf;
using FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Queries;
using FreshGuard.ColdTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.Alerting.Application.Acl;
using FreshGuard.ColdTrack.Platform.Alerting.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Alerting.Application.Internal.CommandServices;
using FreshGuard.ColdTrack.Platform.Alerting.Application.Internal.QueryServices;
using FreshGuard.ColdTrack.Platform.Alerting.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Model.Services;
using FreshGuard.ColdTrack.Platform.Alerting.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.Alerting.Interfaces.Acl;
using FreshGuard.ColdTrack.Platform.Iam.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.CommandServices;
using FreshGuard.ColdTrack.Platform.Iam.Application.Internal.OutboundServices;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using FreshGuard.ColdTrack.Platform.Resources.Errors;
using FreshGuard.ColdTrack.Platform.Resources.Shared;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Initialization;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.Internal.CommandServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.Internal.QueryServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.ShipmentManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.CommandServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.Internal.CommandServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.Internal.QueryServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Application.QueryServices;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Domain.Repositories;
using FreshGuard.ColdTrack.Platform.TelemetryMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using QuestPDF.Infrastructure;
using ProblemDetailsFactory = FreshGuard.ColdTrack.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

var renderPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(renderPort))
    builder.WebHost.UseUrls($"http://0.0.0.0:{renderPort}");

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Add ProblemDetails services
builder.Services.AddProblemDetails();

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Database Connection

// Configure Database Context and route EF logs through the app logger pipeline.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection(TokenSettings.SectionName));
var tokenSettings = builder.Configuration.GetSection(TokenSettings.SectionName).Get<TokenSettings>()
                    ?? throw new InvalidOperationException("JWT settings are not configured.");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = tokenSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// Explicitly register IStringLocalizer for ErrorMessages and Commons
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services
    .AddSingleton<IStringLocalizer<CommonMessages>,
        StringLocalizer<CommonMessages>>();

// Register the custom ProblemDetailsFactory
builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "FreshGuard.ColdTrack.Platform",
            Version = "v1",
            Description = "ColdTrack Platform API",
            TermsOfService = new Uri("https://coldtrack.com/tos"),
            Contact = new OpenApiContact
            {
                Name = "ColdTrack",
                Email = "contact@coldtrack.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        { [new OpenApiSecuritySchemeReference("bearer", document)] = [] });
    options.EnableAnnotations();
});

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Shipment Management Bounded Context
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentCommandService, ShipmentCommandService>();
builder.Services.AddScoped<IShipmentQueryService, ShipmentQueryService>();

// Telemetry Monitoring Bounded Context
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ITelemetryRepository, TelemetryRepository>();
builder.Services.AddScoped<ITelemetryCommandService, TelemetryCommandService>();
builder.Services.AddScoped<ITelemetryQueryService, TelemetryQueryService>();

// Alerting Bounded Context
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<IAlertingContextFacade, AlertingContextFacade>();
builder.Services.AddSingleton(new ThresholdPolicy(
    builder.Configuration.GetValue<decimal>("MonitoringThresholds:MinimumTemperature"),
    builder.Configuration.GetValue<decimal>("MonitoringThresholds:MaximumTemperature"),
    builder.Configuration.GetValue<decimal>("MonitoringThresholds:MaximumHumidity")));

// Analytics and Reporting Bounded Context
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAnalyticsDataSource, AnalyticsDataSource>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
builder.Services.AddScoped<IReportFileService, ReportFileService>();
builder.Services.AddSingleton<IPdfReportGenerator, PdfReportGenerator>();

// Mediator Configuration

// Add Mediator Injection Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));

// Add Cortex Mediator for Event Handling
builder.Services.AddCortexMediator(
    [typeof(Program)]);

var app = builder.Build();

// Apply pending migrations on startup (safe to call even when schema is up to date)
if (app.Configuration.GetValue("Database:InitializeOnStartup", true))
{
    await using var scope = app.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    if (app.Configuration.GetValue("Database:SeedDemoData", true))
        await DatabaseInitializer.SeedDemoDataAsync(services);
}

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
    .AllowAnonymous();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS Policy
app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
