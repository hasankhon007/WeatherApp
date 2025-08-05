public class UserSession
{
    public BotState State { get; set; } = BotState.None;
    public string TempUsername { get; set; }
}
