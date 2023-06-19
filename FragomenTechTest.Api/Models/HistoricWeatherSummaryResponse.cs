namespace FragomenTechTest.Api.Models;

public class HistoricWeatherSummaryResponse
{
    public double AverageTemperature { get; set; }
    public double LowestTemperature { get; set; }
    public double HighestTemperature { get; set; }
    public List<WeatherResponse> Data { get; set; } = new();
}