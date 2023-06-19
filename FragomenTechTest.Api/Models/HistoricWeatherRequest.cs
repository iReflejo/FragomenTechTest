using System.Reflection;

namespace FragomenTechTest.Api.Models;

public class HistoricWeatherRequest
{
    public string? Longitude { get; init; }
    public double? LongitudeDouble => double.TryParse(Longitude, out var longitude) ? longitude : null;

    public string? Latitude { get; init; }
    public double? LatitudeDouble => double.TryParse(Latitude, out var latitude) ? latitude : null;
    
    public string Units { get; init; } = "metric";
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    // Used in minimal APIs to bind the query string to the request object
    public static ValueTask<HistoricWeatherRequest> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var request = new HistoricWeatherRequest
        {
            Latitude = context.Request.Query["latitude"].ToString(),
            Longitude = context.Request.Query["longitude"].ToString(),
            Units = context.Request.Query["units"].ToString(),
        };
        if (DateTime.TryParse(context.Request.Query["startDate"].ToString(), out var dateFrom))
        {
            request.StartDate = dateFrom;
        }
        
        if (DateTime.TryParse(context.Request.Query["endDate"].ToString(), out var dateTo))
        {
            request.EndDate = dateTo;
        }

        return ValueTask.FromResult(request);
    }
}