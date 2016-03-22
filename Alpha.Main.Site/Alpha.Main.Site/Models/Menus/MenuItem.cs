namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Web;
    using App_GlobalResources;
    using Fanex.Common.Helper;
    using Services;

    public class MenuItem : IMenuItem
    {
        private string url;
        private Func<MenuStatus> getMenuStatusFunc;
        private Func<string> getUrlFunc;
        private Func<string, bool> authorizeUrlFunc;

        protected MenuItem() 
            : this(null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null) 
            : this(getMenuStatusFunc, null)
        {           
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Func<string, bool> authorizeUrlFunc)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;            
        }

        public string Key { get; set; }

        public string Name { get; set; }

        public string AppName { get; set; }

        public string DisplayName
        {
            get
            {
                return Alpha_sitemap.ResourceManager.GetString(this.Key);
            }
        }

        public virtual string Url
        {
            get
            {
                var appVirtualPath = AppSettings.GetAppName(AppName);

                if (!string.IsNullOrWhiteSpace(appVirtualPath))
                {
                    return "/" + Regex.Replace(this.url, HttpRuntime.AppDomainAppVirtualPath, appVirtualPath, RegexOptions.IgnoreCase);
                }

                return this.url;
            }

            set
            {
                this.url = value;
            }
        }       

        public virtual MenuStatus Status
        {
            get
            {
                var isAuthorized = authorizeUrlFunc.Invoke(this.url);

                return isAuthorized
                    ? getMenuStatusFunc.Invoke()
                    : new MenuStatus(false, false);
            }
        }

        public bool ShowSubMenus { get; set; }

        public List<IMenuItem> SubMenus { get; set; }
    }
}