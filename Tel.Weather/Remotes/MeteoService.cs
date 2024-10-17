namespace Tel.Weather.Remotes;

public interface IRemoteMeteoService
{
    Forecast? GetForecast(DateOnly forecastDate, City city);
}