using Microsoft.Extensions.DependencyInjection;

using Tel.Weather.Remotes;

namespace Tel.Weather.Extensions;

public static class WeatherDependencyInjection
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddSingleton<IRemoteMeteoService, MeteoSwiss>();
        services.AddSingleton<Forecaster>();

        return services;
    }
}