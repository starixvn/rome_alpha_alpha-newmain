namespace Alpha.Main.Site.Services.Impl
{
    using System.Text;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class SessionValidationService : ISessionValidationService
    {
        public const string UsernameCookieKey = "a8f5f167f44f4811_1";

        public void AddUsernameTokenToCookie()
        {
            var encodedValue = Encode(UserSession.Current.UserName);
            var httpCookie = new HttpCookie(UsernameCookieKey, encodedValue);
            httpCookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        private string Encode(string data)
        {
            var encodedData = Encoding.UTF8.GetBytes(data);
            return HttpServerUtility.UrlTokenEncode(encodedData);
        }
    }
}