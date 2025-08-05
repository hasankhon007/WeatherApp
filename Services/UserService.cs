using Newtonsoft.Json;
using WeatherApp.Models.UserModel;

namespace WeatherApp.Services;

public class UserService
{
    public bool Register(long id, string username, string password)
    {
        var users = LoadUsers();

        if (users.Any(x=> x.TelegramId == id))
        {
            throw new Exception("User already exists");
        }

        users.Add(new User
        {
            TelegramId = id,
            Password = password,
            UserName = username
        });
        SaveUsers(users);
        return true;
    }

    public bool Login(long id, string username, string password)
    {
        var users = LoadUsers();
        return users.Any(x=> x.UserName == username && x.Password == password);
    }
    private List<User> LoadUsers()
    {
        // Agar fayl mavjud bo'lmasa, bo'sh ro'yxatni qaytarish
        if (!File.Exists(PathHolder.UserFIle))
            return new List<User>();

        // Fayldan JSON ni o'qish
        var json = File.ReadAllText(PathHolder.UserFIle);

        // Deserializatsiya qilish (agar null bo'lsa, bo'sh ro'yxat qaytaramiz)
        var users = JsonConvert.DeserializeObject<List<User>>(json);

        // Agar deserializatsiya natijasida null bo'lsa, bo'sh ro'yxatni qaytaring
        return users ?? new List<User>();
    }

    private void SaveUsers(List<User> users)
    {
        var json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(PathHolder.UserFIle, json);
    }
}
