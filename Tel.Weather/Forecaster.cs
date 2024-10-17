using Microsoft.Extensions.Logging;

using Tel.Weather.Remotes;

namespace Tel.Weather;

public sealed class Forecaster(
    IRemoteMeteoService remoteMeteoService,
    ILogger<Forecaster> logger
)
{
    private readonly IRemoteMeteoService _remoteMeteoService =
        remoteMeteoService ?? throw new ArgumentNullException(nameof(remoteMeteoService));

    private readonly ILogger<Forecaster> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

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
            Extensions.LogMessages.FetchingWeatherForecast(_logger, city);

            Forecast? maybeForecast = _remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                Extensions.LogMessages.NoWeatherForecast(_logger, city);

                continue;
            }

            forecasts.Add(maybeForecast);
        }

        return forecasts;
    }
}