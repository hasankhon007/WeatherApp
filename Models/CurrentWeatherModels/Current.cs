using Newtonsoft.Json;

public class ForecastCurrent
{
    [JsonProperty("last_updated_epoch")]
    public long LastUpdatedEpoch { get; set; }

    [JsonProperty("last_updated")]
    public string LastUpdated { get; set; }

    [JsonProperty("temp_c")]
    public float TempC { get; set; }

    [JsonProperty("temp_f")]
    public float TempF { get; set; }

    [JsonProperty("is_day")]
    public int IsDay { get; set; }

    [JsonProperty("condition")]
    public Condition Condition { get; set; }

    [JsonProperty("wind_mph")]
    public float WindMph { get; set; }

    [JsonProperty("wind_kph")]
    public float WindKph { get; set; }

    [JsonProperty("wind_degree")]
    public int WindDegree { get; set; }

    [JsonProperty("wind_dir")]
    public string WindDir { get; set; }

    [JsonProperty("pressure_mb")]
    public float PressureMb { get; set; }

    [JsonProperty("pressure_in")]
    public float PressureIn { get; set; }

    [JsonProperty("precip_mm")]
    public float PrecipMm { get; set; }

    [JsonProperty("precip_in")]
    public float PrecipIn { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("cloud")]
    public int Cloud { get; set; }

    [JsonProperty("feelslike_c")]
    public float FeelslikeC { get; set; }

    [JsonProperty("feelslike_f")]
    public float FeelslikeF { get; set; }

    [JsonProperty("windchill_c")]
    public float WindchillC { get; set; }

    [JsonProperty("windchill_f")]
    public float WindchillF { get; set; }

    [JsonProperty("heatindex_c")]
    public float HeatIndexC { get; set; }

    [JsonProperty("heatindex_f")]
    public float HeatIndexF { get; set; }

    [JsonProperty("dewpoint_c")]
    public float DewPointC { get; set; }

    [JsonProperty("dewpoint_f")]
    public float DewPointF { get; set; }

    [JsonProperty("vis_km")]
    public float VisibilityKm { get; set; }

    [JsonProperty("vis_miles")]
    public float VisibilityMiles { get; set; }

    [JsonProperty("uv")]
    public float UV { get; set; }

    [JsonProperty("gust_mph")]
    public float GustMph { get; set; }

    [JsonProperty("gust_kph")]
    public float GustKph { get; set; }
}
