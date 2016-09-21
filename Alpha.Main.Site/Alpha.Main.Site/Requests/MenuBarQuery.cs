namespace Alpha.Main.Site.Models.Requests
{
    using Main.Models.Enumerations;
    using MediatR;
    using System.Collections.Generic;
    using ViewModels;
    using System.Web;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private List<string> smallDevices = new List<string>()
        {
            "iphone", "ipod", "ipad",
            "android", "palm", "opera mini",
            "blackberry", "windows ce", "iemobile"
        };

        private byte roleId;
        private string userAgent;

        public MenuBarQuery(byte roleId, string userAgent)
        {
            this.roleId = roleId;
            this.userAgent = userAgent;
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.roleId);
            }
        }

        public string UserAgent
        {
            get
            {
                return userAgent;
            }
        }

        public bool IsSmallDevice
        {
            get
            {
                var strUserAgent = (UserAgent ?? string.Empty).ToLower();
                return smallDevices.Exists(str => strUserAgent.Contains(str.ToLower())) || strUserAgent.Contains("version");
            }
        }

        public bool IsCookieLess
        {
            get
            {
                return HttpContext.Current != null ? HttpContext.Current.Session.IsCookieless : false;
            }
        }
    }
}