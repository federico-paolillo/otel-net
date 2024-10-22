using Microsoft.Extensions.Logging;

namespace Tel.Weather.Extensions;

internal static partial class LogMessages
{
    [LoggerMessage(LogLevel.Information, "Pretending to fetch weather forecast for city '{City}'")]
    public static partial void FetchingWeatherForecast(this ILogger logger, City city);

    [LoggerMessage(LogLevel.Warning, "Could not fetch weather forecast for city '{City}'")]
    public static partial void NoWeatherForecast(this ILogger logger, City city);
}