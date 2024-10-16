using Microsoft.AspNetCore.Http.HttpResults;

using Tel.Weather;
using Tel.Web.Endpoints.Weather.Models;

namespace Tel.Web.Endpoints.Weather;

public static class WeatherHandlers
{
    public static Ok<ForecastDto[]> GetWeather(
        HttpContext _,
        Forecaster forecaster,
        DateOnly forecastDate
    )
    {
        ForecastDto[] forecasts = forecaster
            .GetForecasts(forecastDate)
            .Select(ForecastDto.FromForecast)
            .ToArray();

        return TypedResults.Ok(forecasts);
    }
}