using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenWeatherMap.ApiClient.Models;

namespace OpenWeatherMap.ApiClient;

public class OpenWeatherMapClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenWeatherMapClient> _logger;
    private readonly OpenWeatherMapOptions _openWeatherMapOptions;

    public OpenWeatherMapClient(HttpClient httpClient, IOptions<OpenWeatherMapOptions> openWeatherMapOptions, ILogger<OpenWeatherMapClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _openWeatherMapOptions = openWeatherMapOptions.Value;
    }
    
    public async Task<Result<OpenWeatherMapApiResponse>> GetWeatherForecastAsync(OpenWeatherMapRequest request)
    {
        var queryString = $"appid={_openWeatherMapOptions.ApiKey}&units={request.Units}&lon={request.Longitude}&lat={request.Latitude}";

        var url = $"/data/2.5/weather?{queryString}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var weatherForecast = JsonConvert.DeserializeObject<OpenWeatherMapApiResponse>(responseString);
            return weatherForecast!;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Error getting weather forecast");

            return new Result<OpenWeatherMapApiResponse>(e);
        }
    }
    
    public async Task<Result<OpenWeatherMapApiResponse>> GetWeatherForecastSummaryAsync(OpenWeatherMapSummaryRequest request)
    {
        var queryString = $"appid={_openWeatherMapOptions.ApiKey}&units={request.Units}&lon={request.Longitude}&lat={request.Latitude}";

        var url = $"/data/2.5/weather?{queryString}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var weatherForecast = JsonConvert.DeserializeObject<OpenWeatherMapApiResponse>(responseString);
            return weatherForecast!;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Error getting weather forecast");

            return new Result<OpenWeatherMapApiResponse>(e);
        }
    }
}