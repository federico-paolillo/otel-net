using Tel.Weather;

namespace Tel.Web.Endpoints.Weather.Models;

public sealed record ForecastDto(
    DateOnly Date,
    int TemperatureC,
    int TemperatureF,
    string City,
    string Summary
)
{
    public static ForecastDto FromForecast(Forecast forecast)
    {
        return new ForecastDto(
            forecast.Date,
            forecast.TemperatureC,
            forecast.TemperatureF,
            forecast.City.Value,
            forecast.Summary.Value
        );
    }
}