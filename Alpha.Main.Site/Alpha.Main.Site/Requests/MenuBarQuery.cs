namespace Alpha.Main.Site.Models.Requests
{
    using System.Collections.Generic;
    using System.Web;
    using Configurations;
    using Main.Models.Enumerations;
    using MediatR;
    using ViewModels;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private List<string> devicesUseCustomScrollbar;

        public MenuBarQuery(
            byte roleId,
            string userAgent,
            List<string> devicesUseCustomScrollbar = null)
        {
            this.roleId = roleId;
            UserAgent = userAgent;
            this.devicesUseCustomScrollbar = devicesUseCustomScrollbar ?? AppSettings.DevicesUseCustomScrollbar;
        }

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(roleId);

        public string UserAgent { get; private set; }

        public bool IsSmallDevice
        {
            get
            {
                var strUserAgent = (UserAgent ?? string.Empty).ToLower();
                return devicesUseCustomScrollbar.Exists(str => strUserAgent.Contains(str.ToLower())) || strUserAgent.Contains("version");
            }
        }

        public bool IsCookieLess => HttpContext.Current != null ? HttpContext.Current.Session.IsCookieless : false;      
    }
}