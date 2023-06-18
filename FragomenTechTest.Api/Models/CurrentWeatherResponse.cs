namespace FragomenTechTest.Api.Models;

public class CurrentWeatherResponse
{
    public double Clouds { get; set; }
    public double WindSpd { get; set; }
    public string? WindDirection { get; set; }
    public string? Weather { get; set; }
    public double Uv { get; set; }
    public double WindDir { get; set; }
    public double Precip { get; set; }
    public double SolarRad { get; set; }
    public string? CityName { get; set; }
    public string? Sunrise { get; set; }
    public string? Sunset { get; set; }
    public double Temperature { get; set; }
}