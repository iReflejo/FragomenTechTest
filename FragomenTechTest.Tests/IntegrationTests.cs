using System.Net;
using FluentAssertions;
using FragomenTechTest.Api;
using FragomenTechTest.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace FragomenTechTest.Tests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly WebApplicationFactory<IApiMarker> _factory;

    public IntegrationTests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetCurrentWeather_ReturnsWeather()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/weather?longitude=0&latitude=0");
        var content = await result.Content.ReadAsStringAsync();
        var weather = JsonConvert.DeserializeObject<WeatherResponse>(content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        weather.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetCurrentWeather_WithMissingLatitude_ReturnsBadRequest()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/weather?longitude=0");
        var content = await result.Content.ReadAsStringAsync();
        var validationResponse = JsonConvert.DeserializeObject<HttpValidationProblemDetails>(content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationResponse.Should().NotBeNull();
        validationResponse!.Status.Should().Be(StatusCodes.Status400BadRequest);
        validationResponse!.Errors.Should().ContainKey(nameof(CurrentWeatherRequest.Latitude));
    }
    
    [Fact]
    public async Task GetHistoricWeather_ReturnsWeather()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/historic?longitude=0&latitude=0&startDate=2021-01-01&endDate=2021-01-03");
        var content = await result.Content.ReadAsStringAsync();
        var weather = JsonConvert.DeserializeObject<HistoricWeatherSummaryResponse>(content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        weather.Should().NotBeNull();
        weather!.Data.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetHistoricWeather_WithMissingStartDate_ReturnsBadRequest()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/historic?longitude=0&latitude=0&endDate=2021-01-03");
        var content = await result.Content.ReadAsStringAsync();
        var validationResponse = JsonConvert.DeserializeObject<HttpValidationProblemDetails>(content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationResponse.Should().NotBeNull();
        validationResponse!.Status.Should().Be(StatusCodes.Status400BadRequest);
        validationResponse!.Errors.Should().ContainKey(nameof(HistoricWeatherRequest.StartDate));
    }
    
    [Fact]
    public async Task GetHistoricWeather_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/historic?longitude=0&latitude=0&startDate=2021-01-05&endDate=2021-01-03");
        var content = await result.Content.ReadAsStringAsync();
        var validationResponse = JsonConvert.DeserializeObject<HttpValidationProblemDetails>(content);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationResponse.Should().NotBeNull();
        validationResponse!.Status.Should().Be(StatusCodes.Status400BadRequest);
        validationResponse!.Errors.Should().ContainKey(nameof(HistoricWeatherRequest.EndDate));
    }
}