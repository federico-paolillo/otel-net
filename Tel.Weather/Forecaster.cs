using System.Diagnostics;

using Microsoft.Extensions.Logging;

using Tel.Instrument;
using Tel.Weather.Extensions;
using Tel.Weather.Remotes;

namespace Tel.Weather;

public sealed class Forecaster(
    IRemoteMeteoService remoteMeteoService,
    ILogger<Forecaster> logger,
    Instrumentation instrumentation
)
{
    public IReadOnlyList<Forecast> GetForecasts(DateOnly when)
    {
        // One city (Berlin) will not have any data

        City[] cities =
        [
            new("Toronto"),
            new("Berlin"),
            new("Montevideo")
        ];

        using Activity? gettingForecastsActivity = instrumentation.ActivitySource
            .StartActivity("weather.forecasts.get");

        if (gettingForecastsActivity?.IsAllDataRequested == true)
        {
            // Span names and attribute keys should be stable and low-cardinality
            // See: https://opentelemetry.io/docs/specs/otel/trace/api/
            // See: https://opentelemetry.io/docs/specs/semconv/general/naming/

            gettingForecastsActivity.SetTag("tel.weather.city.count", cities.Length);
        }

        List<Forecast> forecasts = [];

        foreach (City city in cities)
        {
            using Activity? getCityForecastsActivity = instrumentation.ActivitySource
                .StartActivity("weather.forecast.get");

            if (getCityForecastsActivity?.IsAllDataRequested == true)
            {
                getCityForecastsActivity.SetTag("tel.weather.city.name", city.Value);
            }

            logger.FetchingWeatherForecast(city);

            Forecast? maybeForecast = remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                logger.NoWeatherForecast(city);

                getCityForecastsActivity?.SetStatus(ActivityStatusCode.Error, "no weather forecast");

                continue;
            }

            forecasts.Add(maybeForecast);

            instrumentation.ForecastsProduced.Add(1);
        }

        return forecasts;
    }
}
