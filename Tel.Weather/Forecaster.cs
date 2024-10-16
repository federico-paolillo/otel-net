using Tel.Weather.Remote;

namespace Tel.Weather;

public sealed class Forecaster
{
    private readonly IRemoteMeteoService _remoteMeteoService;

    public Forecaster(IRemoteMeteoService remoteMeteoService)
    {
        _remoteMeteoService = remoteMeteoService ?? throw new ArgumentNullException(nameof(remoteMeteoService));
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
            Forecast? maybeForecast = _remoteMeteoService.GetForecast(when, city);

            if (maybeForecast is null)
            {
                continue;
            }

            forecasts.Add(maybeForecast);
        }

        return forecasts;
    }
}