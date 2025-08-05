
public class PathHolder
{
    private static readonly string parentRoot = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    public static readonly string UserFIle = Path.Combine(parentRoot, "Database", "users.json");
}
