using FragomenTechTest.Api.Models;
using LanguageExt.Common;

namespace FragomenTechTest.Api.Services;

public interface IWeatherBitService
{
    Task<Result<CurrentWeatherResponse>> GetCurrentWeather(CurrentWeatherRequest request);
}