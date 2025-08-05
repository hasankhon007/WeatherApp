using Newtonsoft.Json;
using System.Collections.Generic;

public class CurrentWeatherModel
{
    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("current")]
    public ForecastCurrent Current { get; set; }
}
