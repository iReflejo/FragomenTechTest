using System.Net;
using System.Threading.RateLimiting;
using FragomenTechTest.Api.Models;

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
}