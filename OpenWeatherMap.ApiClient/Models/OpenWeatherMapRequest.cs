namespace OpenWeatherMap.ApiClient.Models;

public class OpenWeatherMapRequest
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Units { get; init; } = "metric";
}