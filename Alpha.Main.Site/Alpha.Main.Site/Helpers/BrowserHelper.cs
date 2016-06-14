namespace Alpha.Main.Site.Helpers
{
    using System.Web;

    public static class BrowserHelper
    {
        public static bool IsUnsupportedBrowsers()
        {
            var currentRequest = HttpContext.Current.Request;

            if (currentRequest.Browser.Browser.ToLower() == "ie"
                && currentRequest.Browser.MajorVersion <= 9)
            {
                return true;
            }



            return false;
        }
    }
}