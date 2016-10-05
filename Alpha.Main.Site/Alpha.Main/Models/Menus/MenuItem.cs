namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using AppResources;
    using Configurations;

    public class MenuItem : IMenuItem
    {
        private string url;
        private Func<MenuStatus> getMenuStatusFunc;
        private Predicate<string> authorizeUrlFunc;
        private Func<string> getDisplayName;

        protected MenuItem()
            : this(null, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null)
            : this(getMenuStatusFunc, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc)
            : this(getMenuStatusFunc, authorizeUrlFunc, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc, Func<string> getDisplayName)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;
            this.getDisplayName = getDisplayName;
        }

        public string Key { get; set; }

        public string ResourceKey { get; set; }

        public string AppPath { get; set; }

        public bool ByPassAuthorization { get; set; }

        public string DisplayName
        {
            get
            {
                return getDisplayName != null ? getDisplayName() : Alpha_sitemap.ResourceManager.GetString(this.ResourceKey);
            }
        }

        public virtual string Url
        {
            get
            {
                var appVirtualPath = AppSettings.GetAppPath(AppPath);

                if (!string.IsNullOrWhiteSpace(appVirtualPath))
                {
                    return Regex.Replace(this.url, HttpRuntime.AppDomainAppVirtualPath, appVirtualPath);
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
                var isAuthorized = ByPassAuthorization || authorizeUrlFunc(this.Url);

                return isAuthorized
                    ? getMenuStatusFunc()
                    : new MenuStatus(false, false);
            }
        }

        public bool ShowSubMenus { get; set; }

        public List<IMenuItem> SubMenus { get; set; }

        public IEnumerable<IMenuItem> VisibileSubMenus
        {
            get
            {
                return this.SubMenus.Where(menuItem => menuItem.Status.Visible);
            }
        }
    }
}