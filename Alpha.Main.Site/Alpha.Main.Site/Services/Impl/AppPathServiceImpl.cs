namespace Alpha.Main.Site.Services.Impl
{
    using System.Text.RegularExpressions;
    using System.Web;

    public class AppPathServiceImpl : AppPathService
    {
        public string GetAppPath(string appName, string pathModifier)
        {
            var appPath = HttpContext.Current.Response.ApplyAppPathModifier(pathModifier);
            appPath = Regex.Replace(appPath, HttpRuntime.AppDomainAppVirtualPath, appName, RegexOptions.IgnoreCase);
            return appPath;
        }
    }
}