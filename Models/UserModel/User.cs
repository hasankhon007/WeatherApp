using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WeatherApp.Models.UserModel;

public class User
{
    [JsonProperty("telegram_id")]
    public long TelegramId {  get; set; }

    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("password")]
    public string Password {  get; set; }

}
