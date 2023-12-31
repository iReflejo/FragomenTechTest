using FluentValidation;
using FragomenTechTest.Api.Caching;
using FragomenTechTest.Api.Extensions;
using FragomenTechTest.Api.Models;
using FragomenTechTest.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var rateLimitOptions = new RateLimitOptions();
builder.Configuration.GetSection(RateLimitOptions.SectionName).Bind(rateLimitOptions);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCaching(builder.Configuration);

builder.Services.ConfigureOpenWeatherMap(builder.Configuration);
builder.Services.ConfigureWeatherBit(builder.Configuration);

builder.Services.ConfigureRateLimiter(rateLimitOptions);

var app = builder.Build();

// Setup exception handler to return a generic RFC compliant exception response
app.UseExceptionHandler(exceptionHandlerApp 
    => exceptionHandlerApp.Run(async context 
        => await Results.Problem()
            .ExecuteAsync(context)));

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapGet("weather", async (CurrentWeatherRequest request, IWeatherBitService weatherBitService, IValidator<CurrentWeatherRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var response = await weatherBitService.GetCurrentWeather(request);

    return response.Match<IResult>(
        Results.Ok,
        err => Results.StatusCode(500)
    );
});

app.MapGet("historic", async (HistoricWeatherRequest request, IWeatherBitService weatherBitService, IValidator<HistoricWeatherRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var response = await weatherBitService.GetHistoricWeather(request);

    return response.Match<IResult>(
        Results.Ok,
        err => Results.StatusCode(500)
    );
});

app.Run();