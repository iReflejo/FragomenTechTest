namespace OpenWeatherMap.ApiClient.Models;

public class OpenWeatherMapOptions
{
    public const string SectionName = "OpenWeatherMap";
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
}