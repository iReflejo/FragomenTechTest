using Newtonsoft.Json;

namespace WeatherBit.ApiClient.Models;

public class WeatherBitDataResponse
{
    [JsonProperty("wind_cdir")]
    public string WindCdir { get; set; }

    [JsonProperty("rh")]
    public double Rh { get; set; }

    [JsonProperty("pod")]
    public string Pod { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("pres")]
    public double Pres { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    [JsonProperty("ob_time")]
    public string ObTime { get; set; }

    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("clouds")]
    public double Clouds { get; set; }

    [JsonProperty("vis")]
    public double Vis { get; set; }

    [JsonProperty("wind_spd")]
    public double WindSpd { get; set; }

    [JsonProperty("gust")]
    public double? Gust { get; set; }

    [JsonProperty("wind_cdir_full")]
    public string WindCdirFull { get; set; }

    [JsonProperty("app_temp")]
    public double AppTemp { get; set; }

    [JsonProperty("state_code")]
    public string StateCode { get; set; }

    [JsonProperty("ts")]
    public double Ts { get; set; }

    [JsonProperty("h_angle")]
    public double HAngle { get; set; }

    [JsonProperty("dewpt")]
    public double Dewpt { get; set; }

    [JsonProperty("weather")]
    public Weather? Weather { get; set; }

    [JsonProperty("uv")]
    public double Uv { get; set; }

    [JsonProperty("aqi")]
    public double Aqi { get; set; }

    [JsonProperty("station")]
    public string Station { get; set; }

    [JsonProperty("sources")]
    public List<string> Sources { get; set; }

    [JsonProperty("wind_dir")]
    public double WindDir { get; set; }

    [JsonProperty("elev_angle")]
    public double ElevAngle { get; set; }

    [JsonProperty("datetime")]
    public string Datetime { get; set; }

    [JsonProperty("precip")]
    public double Precip { get; set; }

    [JsonProperty("ghi")]
    public double Ghi { get; set; }

    [JsonProperty("dni")]
    public double Dni { get; set; }

    [JsonProperty("dhi")]
    public double Dhi { get; set; }

    [JsonProperty("solar_rad")]
    public double SolarRad { get; set; }

    [JsonProperty("city_name")]
    public string CityName { get; set; }

    [JsonProperty("sunrise")]
    public string Sunrise { get; set; }

    [JsonProperty("sunset")]
    public string Sunset { get; set; }

    [JsonProperty("temp")]
    public double Temp { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("slp")]
    public double Slp { get; set; }
}

public class WeatherBitCurrentResponse
{
    [JsonProperty("data")]
    public List<WeatherBitDataResponse> Data { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }
}

public class Weather
{
    [JsonProperty("icon")]
    public string Icon { get; set; }

    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}