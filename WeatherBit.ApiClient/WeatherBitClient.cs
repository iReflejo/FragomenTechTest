using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherBit.ApiClient.Models;

namespace WeatherBit.ApiClient;

public class WeatherBitClient : IWeatherBitClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherBitClient> _logger;
    private readonly WeatherBitOptions _weatherBitOptions;

    public WeatherBitClient(HttpClient httpClient, IOptions<WeatherBitOptions> weatherBitOptions, ILogger<WeatherBitClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _weatherBitOptions = weatherBitOptions.Value;
    }
    
    public async Task<Result<WeatherBitCurrentResponse>> GetWeatherForecastAsync(WeatherBitRequest request)
    {
        var queryString = $"key={_weatherBitOptions.ApiKey}&units={request.Units}&lon={request.Longitude}&lat={request.Latitude}";

        var url = $"/v2.0/current?{queryString}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var weatherForecast = JsonConvert.DeserializeObject<WeatherBitCurrentResponse>(responseString);
            return weatherForecast!;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Error getting weather forecast");

            return new Result<WeatherBitCurrentResponse>(e);
        }
    }
}