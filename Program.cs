using Telegram.Bot;
using WeatherApp.Services;

namespace WeatherApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string apikey = "a6ad279b1e41463baaf154122252407";
            WeatherService weatherService = new WeatherService(client, apikey);
            TelegramBotClient telegramBotClient = new TelegramBotClient("8392729783:AAHzZHfFLkb5le0oqoFIzlKCQOb0UwNxSoA");
            UserService userService = new UserService();
            MainMenu menu = new MainMenu(telegramBotClient, userService,weatherService);
            await menu.StartAsync();
        }
    }
}