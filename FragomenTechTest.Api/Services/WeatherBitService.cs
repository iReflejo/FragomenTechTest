using FragomenTechTest.Api.Extensions;
using FragomenTechTest.Api.Models;
using LanguageExt.Common;
using WeatherBit.ApiClient;
using WeatherBit.ApiClient.Models;

namespace FragomenTechTest.Api.Services;

public class WeatherBitService : IWeatherBitService
{
    private readonly ILogger<WeatherBitService> _logger;
    private readonly IWeatherBitClient _weatherBitClient;

    public WeatherBitService(ILogger<WeatherBitService> logger, IWeatherBitClient weatherBitClient)
    {
        _logger = logger;
        _weatherBitClient = weatherBitClient;
    }

    public async Task<Result<WeatherResponse>> GetCurrentWeather(CurrentWeatherRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var apiRequest = new WeatherBitRequest
        {
            Latitude = request.LatitudeDouble!.Value,
            Longitude = request.LongitudeDouble!.Value,
            Units = request.Units.ToLower() switch
            {
                "metric" => "m",
                "imperial" => "i",
                "scientific" => "s",
                _ => "m"
            }
        };

        var apiResponse = await _weatherBitClient.GetWeatherForecastAsync(apiRequest);

        return apiResponse.Match(
            r => new Result<WeatherResponse>(r.MapToCurrentWeatherResponse()),
            err => new Result<WeatherResponse>(err)
        );
    }

    public async Task<Result<HistoricWeatherSummaryResponse>> GetHistoricWeather(HistoricWeatherRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var apiRequest = new WeatherBitHistoricRequest()
        {
            Latitude = request.LatitudeDouble!.Value,
            Longitude = request.LongitudeDouble!.Value,
            StartDate = request.StartDate!.Value,
            EndDate = request.EndDate!.Value,
            
            Units = request.Units.ToLower() switch
            {
                "metric" => "m",
                "imperial" => "i",
                "scientific" => "s",
                _ => "m"
            }
        };

        var apiResponse = await _weatherBitClient.GetHistoricWeatherAsync(apiRequest);
        
        return apiResponse.Match(
            r => new Result<HistoricWeatherSummaryResponse>(r.MapToHistoricWeatherResponse()),
            err => new Result<HistoricWeatherSummaryResponse>(err)
        );
    }

}