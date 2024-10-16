namespace Tel.Weather;

public sealed record City(string Value)
{
    public static readonly City[] Defaults =
    [
        new("Toronto"),
        new("New York"),
        new("Monterrey"),
        new("Merida"),
        new("Villa Nueva"),
        new("Tegucigalpa"),
        new("Managua"),
        new("Georgetown"),
        new("Paramaribo"),
        new("Montevideo")
    ];
}