using Microsoft.Extensions.Logging;

using Tel.Weather.Remote;

namespace Tel.Weather;

public sealed class Forecaster
{
    private readonly IRemoteMeteoService _remoteMeteoService;
    private readonly ILogger<Forecaster> _logger;

    public Forecaster(
        IRemoteMeteoService remoteMeteoService,
        ILogger<Forecaster> logger
    )
    {
        _remoteMeteoService = remoteMeteoService ?? throw new ArgumentNullException(nameof(remoteMeteoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IReadOnlyList<Forecast> GetForecasts(DateOnly when)
    {
        // One city (Berlin) will not have any data

        City[] cities =
        [
            new("Toronto"),
            new("Berlin"),
            new("Montevideo")
        ];

        // It is an explicit loop to plug in specific traces later

        List<Forecast> forecasts = [];

        foreach (City city in cities)
        {
            _logger.LogInformation("Pretending to fetch weather forecast for city: '{City}'", city.Value);

            Forecast? maybeForecast = _remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                _logger.LogWarning("No weather forecast returned for '{City}'", city.Value);
                
                continue;
            }

            forecasts.Add(maybeForecast);
        }

        return forecasts;
    }
}