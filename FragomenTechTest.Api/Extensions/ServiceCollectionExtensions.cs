using System.Net;
using System.Threading.RateLimiting;
using FragomenTechTest.Api.Models;
using FragomenTechTest.Api.Services;
using OpenWeatherMap.ApiClient;
using OpenWeatherMap.ApiClient.Models;
using WeatherBit.ApiClient;
using WeatherBit.ApiClient.Models;

namespace FragomenTechTest.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureRateLimiter(this IServiceCollection services, RateLimitOptions rateLimitOptions)
    {
        // Configure the default microsoft rate limiter implementation
        services.AddRateLimiter(options =>
        {
            // Override default status code from 503 to 429
            options.RejectionStatusCode = (int) HttpStatusCode.TooManyRequests;

            // Create a rate limit 'bucket' based on IP address. 
            // In a production environment this would be based on client credentials
            // For the purpose of this piece of work it should suffice
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                
                // remote IP is null in integration tests
                // If the request is from localhost, don't rate limit as key doesnt work
                if (remoteIpAddress is null || IPAddress.IsLoopback(remoteIpAddress))
                {
                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                }

                return RateLimitPartition.GetFixedWindowLimiter
                (remoteIpAddress!, _ => new FixedWindowRateLimiterOptions
                {
                    Window = TimeSpan.FromSeconds(rateLimitOptions.Window),
                    PermitLimit = rateLimitOptions.PermitLimit,
                    QueueLimit = rateLimitOptions.QueueLimit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
        });
    }
    
    public static void ConfigureOpenWeatherMap(this IServiceCollection services, IConfiguration configuration)
    {
        var openWeatherMapOptions = new OpenWeatherMapOptions();
        configuration.GetSection(OpenWeatherMapOptions.SectionName).Bind(openWeatherMapOptions);
        services.AddOptions<OpenWeatherMapOptions>().Bind(configuration.GetSection(OpenWeatherMapOptions.SectionName));
        services.AddHttpClient<OpenWeatherMapClient>(options =>
        {
            if (openWeatherMapOptions.BaseUrl == null)
            {
                throw new Exception($"{nameof(OpenWeatherMapOptions)} configuration is missing {nameof(OpenWeatherMapOptions.BaseUrl)}");
            }
            options.BaseAddress = new Uri(openWeatherMapOptions.BaseUrl);
        });
    }
    
    public static void ConfigureWeatherBit(this IServiceCollection services, IConfiguration configuration)
    {
        var weatherBitOptions = new WeatherBitOptions();
        configuration.GetSection(WeatherBitOptions.SectionName).Bind(weatherBitOptions);
        services.AddOptions<WeatherBitOptions>().Bind(configuration.GetSection(WeatherBitOptions.SectionName));
        services.AddHttpClient<IWeatherBitClient, WeatherBitClient>(options =>
        {
            if (string.IsNullOrWhiteSpace(weatherBitOptions.BaseUrl))
            {
                throw new Exception($"{nameof(OpenWeatherMapOptions)} configuration is missing {nameof(OpenWeatherMapOptions.BaseUrl)}");
            }
            options.BaseAddress = new Uri(weatherBitOptions.BaseUrl);
        });

        services.AddTransient<IWeatherBitService, WeatherBitService>();
    }
}