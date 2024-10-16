using Tel.Weather;
using Tel.Web.Endpoints.Weather;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddWeatherServices();

WebApplication app = builder.Build();

app.MapWeatherEndpoints();

app.Run();