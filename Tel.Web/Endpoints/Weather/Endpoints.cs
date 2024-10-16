namespace Tel.Web.Endpoints.Weather;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder builder = app.MapGroup("/weather");

        builder.MapGet("/", WeatherHandlers.GetWeather);

        return builder;
    }
}