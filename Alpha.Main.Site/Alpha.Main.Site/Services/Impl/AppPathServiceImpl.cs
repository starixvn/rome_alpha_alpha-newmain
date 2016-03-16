namespace Alpha.Main.Site.Services.Impl
{
    using System.Text.RegularExpressions;
    using System.Web;

    public class AppPathServiceImpl : AppPathService
    {
        public string GetAppPath(string appName, string pathModifier)
        {
            var redirectUrl = HttpContext.Current.Response.ApplyAppPathModifier(pathModifier);
            redirectUrl = Regex.Replace(redirectUrl, HttpRuntime.AppDomainAppVirtualPath, appName, RegexOptions.IgnoreCase);
            return redirectUrl;
        }
    }
}