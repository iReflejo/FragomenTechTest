using LanguageExt.Common;
using WeatherBit.ApiClient.Models;

namespace WeatherBit.ApiClient;

public interface IWeatherBitClient
{
    Task<Result<WeatherBitCurrentResponse>> GetWeatherForecastAsync(WeatherBitRequest request);
}