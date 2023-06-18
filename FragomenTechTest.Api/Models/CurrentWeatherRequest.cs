using System.Reflection;

namespace FragomenTechTest.Api.Models;

public class CurrentWeatherRequest
{
    public string? Longitude { get; init; }
    public double? LongitudeDouble => double.TryParse(Longitude, out var longitude) ? longitude : null;

    public string? Latitude { get; init; }
    public double? LatitudeDouble => double.TryParse(Latitude, out var latitude) ? latitude : null;
    
    public string Units { get; init; } = "metric";
    
    // Used in minimal APIs to bind the query string to the request object
    public static ValueTask<CurrentWeatherRequest> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var request = new CurrentWeatherRequest
        {
            Latitude = context.Request.Query["latitude"].ToString(),
            Longitude = context.Request.Query["longitude"].ToString(),
            Units = context.Request.Query["units"].ToString()
        };

        return ValueTask.FromResult(request);
    }
}