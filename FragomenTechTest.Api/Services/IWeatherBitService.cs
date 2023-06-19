using FragomenTechTest.Api.Models;
using LanguageExt.Common;

namespace FragomenTechTest.Api.Services;

public interface IWeatherBitService
{
    Task<Result<WeatherResponse>> GetCurrentWeather(CurrentWeatherRequest request);
    Task<Result<HistoricWeatherSummaryResponse>> GetHistoricWeather(HistoricWeatherRequest request);
}