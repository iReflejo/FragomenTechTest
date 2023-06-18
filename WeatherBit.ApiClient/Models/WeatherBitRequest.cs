namespace WeatherBit.ApiClient.Models;

public class WeatherBitRequest
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Units { get; init; } = "m";
}