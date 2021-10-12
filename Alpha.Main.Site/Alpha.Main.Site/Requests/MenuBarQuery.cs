namespace Alpha.Main.Site.Models.Requests
{
    using Alpha.Web.Models;
    using Configurations;
    using MediatR;
    using System.Collections.Generic;
    using System.Web;
    using ViewModels;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private List<string> devicesUseCustomScrollbar;

        public MenuBarQuery(
            byte roleId,
            string userAgent,
            List<string> devicesUseCustomScrollbar = null,
            string darkModeName = null)
        {
            UserLevel = Enumeration.FromValue<UserLevel>(roleId);
            UserAgent = userAgent;
            this.devicesUseCustomScrollbar = devicesUseCustomScrollbar ?? AppSettings.DevicesUseCustomScrollbar;
            DarkModeName = darkModeName;
        }

        public UserLevel UserLevel { get; }

        public string UserAgent { get; }

        public bool IsSmallDevice
        {
            get
            {
                var strUserAgent = (UserAgent ?? string.Empty).ToLower();
                return devicesUseCustomScrollbar.Exists(str => strUserAgent.Contains(str.ToLower())) || strUserAgent.Contains("version");
            }
        }

        public bool IsCookieLess => HttpContext.Current != null ? HttpContext.Current.Session.IsCookieless : false;

        public string DarkModeName { get; }
    }
}