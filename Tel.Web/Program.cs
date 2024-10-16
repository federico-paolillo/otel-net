using Tel.Weather;
using Tel.Web;
using Tel.Web.Endpoints.Weather;

var builder = WebApplication.CreateBuilder(args);

// using var loggerFactory = LoggerFactory.Create(bld => bld.UseSimpleLogger());
//
// var startupLogger = loggerFactory.CreateLogger<Program>();

builder.Logging.UseSimpleLogger();

builder.UseOpenTelemetry();

builder.Services.AddWeatherServices();

var app = builder.Build();

app.MapWeatherEndpoints();

app.Run();