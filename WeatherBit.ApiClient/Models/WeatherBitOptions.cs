namespace WeatherBit.ApiClient.Models;

public class WeatherBitOptions
{
    public const string SectionName = "WeatherBit";
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
}