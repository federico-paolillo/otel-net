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
            .StartActivity("getting weather forecasts");

        for (int i = 0; i < cities.Length; i++)
        {
            gettingForecastsActivity?.AddTag($"city_{i:D}", cities[i].Value);
        }

        List<Forecast> forecasts = [];

        foreach (City city in cities)
        {
            using Activity? getCityForecastsActivity = instrumentation.ActivitySource
                .StartActivity($"getting weather for '{city.Value}'");

            logger.FetchingWeatherForecast(city);

            Forecast? maybeForecast = remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                logger.NoWeatherForecast(city);

                getCityForecastsActivity?.SetStatus(ActivityStatusCode.Error, "no weather forecast");

                continue;
            }

            forecasts.Add(maybeForecast);

            instrumentation.WheaterRequests.Add(1);
        }

        return forecasts;
    }
}