using FragomenTechTest.Api.Models;
using WeatherBit.ApiClient.Models;

namespace FragomenTechTest.Api.Extensions;

public static class WeatherBitResponseExtensions
{
    public static CurrentWeatherResponse MapToCurrentWeatherResponse(this WeatherBitCurrentResponse response)
    {
        if(response.Data == null || response.Data.Count == 0)
        {
            throw new Exception("No data returned from WeatherBit API");
        }
        
        // Get the first day of data
        var data = response.Data[0];

        return new CurrentWeatherResponse
        {
            Clouds = data.Clouds,
            WindSpd = data.WindSpd,
            WindDirection = data.WindCdirFull,
            Precip = data.Precip,
            SolarRad = data.SolarRad,
            Sunrise = data.Sunrise,
            Sunset = data.Sunset,
            Temperature = data.Temp,
            Uv = data.Uv,
            Weather = data.Weather?.Description,
            CityName = data.CityName,
            WindDir = data.WindDir
        };
    }
}