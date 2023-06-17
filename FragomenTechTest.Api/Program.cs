using FragomenTechTest.Api.Extensions;
using FragomenTechTest.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var rateLimitOptions = new RateLimitOptions();
builder.Configuration.GetSection(RateLimitOptions.SectionName).Bind(rateLimitOptions);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.ConfigureRateLimiter(rateLimitOptions);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();