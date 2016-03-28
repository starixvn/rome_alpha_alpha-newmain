namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

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