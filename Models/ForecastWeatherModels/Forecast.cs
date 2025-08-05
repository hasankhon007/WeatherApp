using Newtonsoft.Json;

public class Forecast
{
    [JsonProperty("forecastday")]
    public List<ForecastDay> ForecastDays { get; set; }
}
