using Microsoft.Extensions.DependencyInjection;

using Tel.Weather.Remote;

namespace Tel.Weather;

public static class WeatherDependencyInjection
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddSingleton<IRemoteMeteoService, MeteoSwiss>();
        services.AddSingleton<Forecaster>();

        return services;
    }
}