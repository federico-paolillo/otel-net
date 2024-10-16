using Microsoft.Extensions.Logging.Console;

using Tel.Weather;
using Tel.Web.Endpoints.Weather;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddWeatherServices();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(opts =>
{
    opts.SingleLine = true;
    opts.IncludeScopes = false;
    opts.UseUtcTimestamp = true;
    opts.TimestampFormat="yyyy-MM-dd HH:mm:ss";
    opts.ColorBehavior = LoggerColorBehavior.Disabled;
});

WebApplication app = builder.Build();

app.MapWeatherEndpoints();

app.Run();