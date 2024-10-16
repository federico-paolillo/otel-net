namespace Tel.Weather;

public sealed record Forecast(DateOnly Date, int TemperatureC, City City, Summary Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}