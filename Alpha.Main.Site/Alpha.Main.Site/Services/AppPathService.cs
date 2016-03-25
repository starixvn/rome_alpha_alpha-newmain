namespace Alpha.Main.Site.Services
{
    public interface AppPathService
    {
        string GetAppPath(string appName, string pathModifier);

        string GetContentPath(string relativePath);
    }
}
