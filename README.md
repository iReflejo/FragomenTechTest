
# Weather API - Tech Test

This .Net weather API provides functionality to retrieve a weather forecast using WeatherBit as the source. 

## Prerequisites

[.Net Core 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

Optional 
- C# IDE (Visual Studio, Rider, Visual Studio Code)

## Setup

To run the application first clone it to a local directory. 

You then need to enter your WeatherBit API key into the appsettings key 'WeatherBit:ApiKey' located at: {SolutionRoot}\FragomenTechTest.Api\appsettings.json 

You can than either run the application using the [the .NET Core SDK](https://www.microsoft.com/net/download). :

```console
dotnet build
dotnet run
```

Or you can run it through the relevant C# IDE.

## Usage

### /weather
#### Query Parameters

```console
longitude: double
latitude: double

-Optional-
units: string (M/I/S default: M)
```

This endpoint returns a weather forecast for a given location.

### /historic
#### Query Parameters

```console
longitude: double
latitude: double
startDate: DateTime
endDate: DateTime

-Optional-
units: string (M/I/S default: M)
```

This endpoint returns a historic weather forecast for a given location and date range. With a summary over the entire range