using LanguageExt.Common;
using WeatherBit.ApiClient.Models;

namespace WeatherBit.ApiClient;

public interface IWeatherBitClient
{
    Task<Result<WeatherBitResponse>> GetWeatherForecastAsync(WeatherBitRequest request);
    Task<Result<WeatherBitResponse>> GetHistoricWeatherAsync(WeatherBitHistoricRequest request);
}