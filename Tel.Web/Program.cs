using Tel.Instrument;
using Tel.Weather.Extensions;
using Tel.Web.Endpoints.Weather;
using Tel.Web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

using ILoggerFactory loggerFactory = LoggerFactory.Create(bld => bld.UseSimpleLogger());

// It is common to have an extra logging pipeline to log information during initialization -
// that is: when the real logger pipeline with OpenTelemetry is not yet in place
// See: https://it.wikipedia.org/wiki/Quis_custodiet_ipsos_custodes%3F
// See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/cddc09127f49143de1dc54800b65b710c1f6056d/docs/logs/getting-started-aspnetcore/README.md?plain=1#L125

ILogger<Program> startupLogger = loggerFactory.CreateLogger<Program>();

builder.Logging.UseSimpleLogger();

builder.UseOpenTelemetry(startupLogger);

builder.Services.AddWeatherServices();

WebApplication app = builder.Build();

app.MapWeatherEndpoints();

try
{
    startupLogger.HostStarting();

    app.Run();

    startupLogger.HostQuitting();
}
catch (Exception ex)
{
    startupLogger.HostFailure(ex);

    Environment.Exit(1);
}