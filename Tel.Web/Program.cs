using Tel.Weather;
using Tel.Weather.Extensions;
using Tel.Web;
using Tel.Web.Endpoints.Weather;
using Tel.Web.Extensions;

using LogMessages = Tel.Web.Extensions.LogMessages;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(bld => bld.UseSimpleLogger());

var startupLogger = loggerFactory.CreateLogger<Program>();

builder.Logging.UseSimpleLogger();

builder.UseOpenTelemetry(startupLogger);

builder.Services.AddWeatherServices();

var app = builder.Build();

app.MapWeatherEndpoints();

try
{
    LogMessages.HostStarting(startupLogger);

    app.Run();

    LogMessages.HostQuitting(startupLogger);
}
catch (Exception ex)
{
    LogMessages.HostFailure(startupLogger, ex);

    Environment.Exit(1);
}