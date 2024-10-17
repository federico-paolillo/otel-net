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
    private readonly IRemoteMeteoService _remoteMeteoService =
        remoteMeteoService ?? throw new ArgumentNullException(nameof(remoteMeteoService));

    private readonly ILogger<Forecaster> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly Instrumentation _instrumentation =
        instrumentation ?? throw new ArgumentNullException(nameof(instrumentation));

    public IReadOnlyList<Forecast> GetForecasts(DateOnly when)
    {
        // One city (Berlin) will not have any data

        City[] cities =
        [
            new("Toronto"),
            new("Berlin"),
            new("Montevideo")
        ];

        using var gettingForecastsActivity = instrumentation.ActivitySource
            .StartActivity("getting weather forecasts");

        for (var i = 0; i < cities.Length; i++)
        {
            gettingForecastsActivity?.AddTag($"city_{i:D}", cities[i].Value);
        }

        List<Forecast> forecasts = [];

        foreach (City city in cities)
        {
            using var getCityForecastsActivity = instrumentation.ActivitySource
                .StartActivity($"getting weather for '{city.Value}'");
            
            _logger.FetchingWeatherForecast(city);

            var maybeForecast = _remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                _logger.NoWeatherForecast(city);
                
                getCityForecastsActivity?.SetStatus(ActivityStatusCode.Error, "no weather forecast");

                continue;
            }

            forecasts.Add(maybeForecast);
        }

        return forecasts;
    }
}