namespace Tel.Weather.Remote;

public interface IRemoteMeteoService
{
    Forecast? GetForecast(DateOnly forecastDate, City city);
}