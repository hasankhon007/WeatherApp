using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherApp.Services;

public class MainMenu
{
    private readonly ITelegramBotClient _bot;
    private readonly UserService _userService;
    private readonly Dictionary<long, UserSession> _sessions;
    private readonly WeatherService _weatherService;

    public MainMenu(ITelegramBotClient bot, UserService userService, WeatherService weatherService)
    {
        _bot = bot;
        _userService = userService;
        _sessions = new Dictionary<long, UserSession>();
        _weatherService = weatherService;
    }
        public async Task StartAsync()
        {
            using var cts = new CancellationTokenSource();
            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions: new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery } },
                cancellationToken: cts.Token
            );

            var me = await _bot.GetMe(cts.Token);
            Console.WriteLine($"✅ User Bot @{me.Username} ishlashni boshladi.");
            await Task.Delay(-1, cts.Token);
        }

    private  Task HandleErrorAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {exception.Message}");
        

        if (exception.InnerException != null)
            Console.WriteLine($"[INNER EXCEPTION] {exception.InnerException.Message}");

        Console.ResetColor();
        return Task.CompletedTask;
    }

   private async Task HandleUpdateAsync(ITelegramBotClient bot,Update update, CancellationToken token)
    {
        
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
                await HandleMessageAsync(update.Message);
       
    }

    private async Task HandleMessageAsync(Message message)
    {
        var chatId = message.Chat.Id;
        var text = message.Text?.Trim() ?? "";

        if (!_sessions.ContainsKey(chatId))
            _sessions[chatId] = new UserSession();

        var session = _sessions[chatId];

        switch (text.ToLower())
        {
            case "/start":
                session.State = BotState.None;
                await ShowStartMenu(chatId);
                break;

            case "register":
                session.State = BotState.Register_Username;
                await _bot.SendMessage(chatId, "📝 Enter a username:");
                break;

            case "login":
                session.State = BotState.Login_Username;
                await _bot.SendMessage(chatId, "🔐 Enter your username:");
                break;

            case "🌤 current weather":
                session.State = BotState.EnterCity_CurrentWeather;
                await _bot.SendMessage(chatId, "📍 Enter a city (e.g., Tashkent):");
                break;

            case "📅 3-day forecast":
                session.State = BotState.EnterCity_Forecast;
                await _bot.SendMessage(chatId, "📍 Enter a city (e.g., Tashkent):");
                break;

            default:
                await HandleStateFlow(chatId, text, session);
                break;
        }
    }

    private async Task ShowStartMenu(long chatId)
    {
        var markup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Register", "Login" }
        })
        {
            ResizeKeyboard = true
        };

        await _bot.SendMessage(chatId, "👋 Welcome to WeatherBot!\nPlease register or login:", replyMarkup: markup);
    }

    private async Task ShowWeatherMenu(long chatId)
    {
        var markup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "🌤 Current Weather", "📅 3-Day Forecast" }
        })
        {
            ResizeKeyboard = true
        };

        await _bot.SendMessage(chatId, "✅ You're logged in!\nChoose an option:", replyMarkup: markup);
    }

    private async Task HandleStateFlow(long chatId, string text, UserSession session)
    {
        switch (session.State)
        {
            case BotState.Register_Username:
                session.TempUsername = text;
                session.State = BotState.Register_Password;
                await _bot.SendMessage(chatId, "🔑 Enter a password:");
                break;

            case BotState.Register_Password:
                var registered = _userService.Register(chatId, session.TempUsername, text);
                session.State = BotState.None;
                await _bot.SendMessage(chatId, registered ? "✅ Registered!" : "❌ Username taken.");
                if (registered) await ShowWeatherMenu(chatId);
                break;

            case BotState.Login_Username:
                session.TempUsername = text;
                session.State = BotState.Login_Password;
                await _bot.SendMessage(chatId, "🔑 Enter your password:");
                break;

            case BotState.Login_Password:
                var loggedIn = _userService.Login(chatId, session.TempUsername, text);
                session.State = BotState.None;
                await _bot.SendMessage(chatId, loggedIn ? "✅ Logged in!" : "❌ Wrong credentials.");
                if (loggedIn) await ShowWeatherMenu(chatId);
                break;

            case BotState.EnterCity_CurrentWeather:
                session.State = BotState.None;
                await ShowCurrentWeather(chatId, text);
                break;

            case BotState.EnterCity_Forecast:
                session.State = BotState.None;
                await ShowForecast(chatId, text);
                break;

            default:
                await _bot.SendMessage(chatId, "🤖 Unknown command. Use /start to begin.");
                break;
        }
    }

    private async Task ShowCurrentWeather(long chatId, string city)
    {
        var data = _weatherService.GetFormattedCurrentAsync(city);
        if (data == null)
        {
            await _bot.SendMessage(chatId, "⚠️ Error parsing data.");
            return;
        }

        
        await _bot.SendMessage(chatId, data.Result, parseMode: ParseMode.Markdown);
    }

    private async Task ShowForecast(long chatId, string city)
    {
        
        var data = _weatherService.GetFormattedForecastAsync(city);
        if (data == null)
        {
            await _bot.SendMessage(chatId, "⚠️ Error parsing forecast.");
            return;
        }


        await _bot.SendMessage(chatId, data.Result, parseMode: ParseMode.Markdown);
    }

}
