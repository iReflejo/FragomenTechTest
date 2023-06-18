namespace OpenWeatherMap.ApiClient.Models;

public class OpenWeatherMapSummaryRequest
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Units { get; init; } = "metric";
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}