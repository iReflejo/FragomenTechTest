namespace WeatherBit.ApiClient.Models;

public class WeatherBitHistoricRequest
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Units { get; init; } = "m";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}