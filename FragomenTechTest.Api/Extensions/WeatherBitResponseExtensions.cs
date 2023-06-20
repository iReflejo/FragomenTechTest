using System.Globalization;
using FragomenTechTest.Api.Models;
using WeatherBit.ApiClient.Models;

namespace FragomenTechTest.Api.Extensions;

public static class WeatherBitResponseExtensions
{
    public static WeatherResponse MapToCurrentWeatherResponse(this WeatherBitResponse response)
    {
        if(response.Data == null || response.Data.Count == 0)
        {
            throw new Exception("No data returned from WeatherBit API");
        }
        
        // Get the first day of data
        var data = response.Data[0];

        return MapSingleWeatherResponse(data);
    }
    
    public static HistoricWeatherSummaryResponse MapToHistoricWeatherResponse(this WeatherBitResponse response)
    {
        if(response.Data == null || response.Data.Count == 0)
        {
            throw new Exception("No data returned from WeatherBit API");
        }

        var data = response.Data
            .Select(MapSingleWeatherResponse).ToList();
        
        return new HistoricWeatherSummaryResponse
        {
            AverageTemperature = data.Average(x => x.Temperature),
            LowestTemperature = data.Min(x => x.Temperature),
            HighestTemperature = data.Max(x => x.Temperature),
            Data = data
        };
    }

    private static WeatherResponse MapSingleWeatherResponse(WeatherBitDataResponse data)
    {
        DateTime.TryParseExact(data.DateTime, "yyyy-MM-dd:hh", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);
        return new WeatherResponse
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
            WindDir = data.WindDir,
            DateTime = dateTime
        };
    }

}