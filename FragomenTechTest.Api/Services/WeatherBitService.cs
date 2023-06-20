using FragomenTechTest.Api.Caching;
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
    private readonly IFragomenMemoryCache _caching;

    public WeatherBitService(ILogger<WeatherBitService> logger, IWeatherBitClient weatherBitClient, IFragomenMemoryCache caching)
    {
        _logger = logger;
        _weatherBitClient = weatherBitClient;
        _caching = caching;
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

         // Build a unique cache key based on the request parameters
        var cacheKey = CacheKey.With<WeatherBitService>(nameof(GetCurrentWeather), apiRequest.Longitude,
            apiRequest.Latitude, apiRequest.Units);
        
        // Check the cache, pass in a delegate to create the value if it doesn't exist
        return await _caching.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
        {
            var apiResponse = await _weatherBitClient.GetWeatherForecastAsync(apiRequest);

            return apiResponse.Match(
                r => new Result<WeatherResponse>(r.MapToCurrentWeatherResponse()),
                err => new Result<WeatherResponse>(err));
        }, false);
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
        
        var cacheKey = CacheKey.With<WeatherBitService>(nameof(GetHistoricWeather), apiRequest.Longitude,
            apiRequest.Latitude, apiRequest.Units, apiRequest.StartDate, apiRequest.EndDate);
        return await _caching.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
        {
            var apiResponse = await _weatherBitClient.GetHistoricWeatherAsync(apiRequest);
        
            return apiResponse.Match(
                r => new Result<HistoricWeatherSummaryResponse>(r.MapToHistoricWeatherResponse()),
                err => new Result<HistoricWeatherSummaryResponse>(err)
            );
        }, false);
    }

}