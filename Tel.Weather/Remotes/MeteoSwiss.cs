namespace Tel.Weather.Remotes;

public sealed class MeteoSwiss : IRemoteMeteoService
{
    public Forecast? GetForecast(DateOnly forecastDate, City city)
    {
        if (City.Defaults.Contains(city))
        {
            Thread.Sleep(Random.Shared.Next(0, 100));

            return RandomForecast(forecastDate, city);
        }

        return null;
    }

    private static Forecast RandomForecast(DateOnly when, City city)
    {
        return new Forecast
        (
            when,
            Random.Shared.Next(-20, 55),
            city,
            Summary.Defaults[Random.Shared.Next(Summary.Defaults.Length)]
        );
    }
}