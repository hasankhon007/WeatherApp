using Newtonsoft.Json;
using System.Collections.Generic;

public class ForecastWeatherModel
{
    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("current")]
    public Current Current { get; set; }

    [JsonProperty("forecast")]
    public Forecast Forecast { get; set; }
}
