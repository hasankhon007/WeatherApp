using Newtonsoft.Json;
using System.Text;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public WeatherService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    private async Task<CurrentWeatherModel?> GetCurrentWeatherAsync(string city)
    {
        string url = $"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}&aqi=no";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CurrentWeatherModel>(json);
    }

    private async Task<ForecastWeatherModel?> GetForecastAsync(string city, int days = 3)
    {
        string url = $"http://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city}&days={days}&aqi=no&alerts=no";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ForecastWeatherModel>(json);
    }

    public async Task<string> GetFormattedCurrentAsync(string city)
    {
        var data = await GetCurrentWeatherAsync(city);
        if (data == null) return "❌ City not found or API error.";

        return $"📍 *{data.Location.Name}*\n" +
               $"🌡 Temp: {data.Current.TempC}°C\n" +
               $"💧 Humidity: {data.Current.Humidity}%\n" +
               $"🌬 Wind: {data.Current.WindKph} kph {data.Current.WindDir}\n" +
               $"☁️ Condition: {data.Current.Condition.Text}\n" +
               $"🕒 Local Time: {data.Location.Localtime}";
    }

    public async Task<string> GetFormattedForecastAsync(string city, int days = 3)
    {
        var forecast = await GetForecastAsync(city, days);
        if (forecast == null) return "❌ City not found or API error.";

        var sb = new StringBuilder();
        sb.AppendLine($"📅 *{days}-Day Forecast for {forecast.Location.Name}*");

        foreach (var day in forecast.Forecast.ForecastDays)
        {
            sb.AppendLine($"\n📆 *{day.Date}*");
            sb.AppendLine($"🌤 {day.Day.Condition.Text}");
            sb.AppendLine($"🌡 Max: {day.Day.MaxTempC}°C | Min: {day.Day.MinTempC}°C | Avg: {day.Day.AvgTempC}°C");
        }

        return sb.ToString();
    }
}
