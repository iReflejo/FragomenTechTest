using FluentAssertions;
using FragomenTechTest.Api.Caching;
using FragomenTechTest.Api.Models;
using FragomenTechTest.Api.Services;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherBit.ApiClient;
using WeatherBit.ApiClient.Models;
using Xunit;

namespace FragomenTechTest.Tests;

public class WeatherBitServiceTests
{
    private readonly ILogger<WeatherBitService> _logger;
    private readonly IFragomenMemoryCache _caching;
    
    public WeatherBitServiceTests()
    {
        _logger = Mock.Of<ILogger<WeatherBitService>>();
        _caching = Mock.Of<IFragomenMemoryCache>(c => c.CreateEntry(It.IsAny<object>()) == Mock.Of<ICacheEntry>());
    }
    
    [Fact]
    public async Task GetCurrentWeather_ReturnsWeather()
    {
        // Arrange
        var mockedClient = new Mock<IWeatherBitClient>();
        mockedClient.Setup(x => x.GetWeatherForecastAsync(It.IsAny<WeatherBitRequest>()))
            .ReturnsAsync(new Result<WeatherBitResponse>(GetSuccessfulApiResponse()));
        
        var weatherBitService = new WeatherBitService(_logger, mockedClient.Object, _caching);
        var request = new CurrentWeatherRequest
        {
            Longitude = "0",
            Latitude = "0"
        };
        
        // Act
        var response = await weatherBitService.GetCurrentWeather(request);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.IfSucc(w =>
        {
            w.Temperature.Should().Be(10);
        });
    }
    
    [Fact]
    public async Task GetCurrentWeather_WithApiException_ReturnsExceptionResult()
    {
        // Arrange
        var mockedClient = new Mock<IWeatherBitClient>();
        mockedClient.Setup(x => x.GetWeatherForecastAsync(It.IsAny<WeatherBitRequest>()))
            .ReturnsAsync(new Result<WeatherBitResponse>(new Exception("Error getting weather forecast")));
        
        var weatherBitService = new WeatherBitService(_logger, mockedClient.Object, _caching);
        var request = new CurrentWeatherRequest
        {
            Longitude = "0",
            Latitude = "0"
        };
        
        // Act
        var response = await weatherBitService.GetCurrentWeather(request);

        // Assert
        response.IsFaulted.Should().BeTrue();
        response.IfFail(w =>
        {
            w.Message.Should().Be("Error getting weather forecast");
        });
    }

    private WeatherBitResponse GetSuccessfulApiResponse()
    {
        return new WeatherBitResponse
        {
            Count = 1,
            Data = new List<WeatherBitDataResponse>
            {
                new WeatherBitDataResponse
                {
                    Temp = 10,
                }
            }
        };
    }
}