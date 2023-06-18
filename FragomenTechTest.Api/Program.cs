using FluentValidation;
using FragomenTechTest.Api.Extensions;
using FragomenTechTest.Api.Models;
using FragomenTechTest.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var rateLimitOptions = new RateLimitOptions();
builder.Configuration.GetSection(RateLimitOptions.SectionName).Bind(rateLimitOptions);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.ConfigureOpenWeatherMap(builder.Configuration);
builder.Services.ConfigureWeatherBit(builder.Configuration);

builder.Services.ConfigureRateLimiter(rateLimitOptions);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapGet("weather", async (CurrentWeatherRequest request, IWeatherBitService weatherBitService, IValidator<CurrentWeatherRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    var response = await weatherBitService.GetCurrentWeather(request);

    return response.Match<IResult>(
        Results.Ok,
        err => Results.StatusCode(500)
    );
});

app.Run();