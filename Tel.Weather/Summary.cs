namespace Tel.Weather;

public sealed record Summary(string Value)
{
    public static readonly Summary[] Defaults =
    [
        new("Freezing"),
        new("Bracing"),
        new("Chilly"),
        new("Cool"),
        new("Mild"),
        new("Warm"),
        new("Balmy"),
        new("Hot"),
        new("Sweltering"),
        new("Scorching")
    ];
}