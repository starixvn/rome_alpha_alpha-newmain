namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using AppResources;
    using Configurations;
    using Sunplus.Agent.Model.Entities;

    public class MenuItem : IMenuItem
    {
        private string link;
        private Func<MenuStatus> getMenuStatusFunc;
        private Predicate<string> authorizeUrlFunc;
        private Func<string> getDisplayName;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;
        private readonly Func<bool> isHideForSite;

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null)
            : this(getMenuStatusFunc, null, null, null, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc)
            : this(getMenuStatusFunc, authorizeUrlFunc, null, null, null, null)
        {
        }

        public MenuItem(
            Func<MenuStatus> getMenuStatusFunc,
            Predicate<string> authorizeUrlFunc,
            Func<string> getDisplayName,
            Func<bool> isProtectedModeFunc,
            Func<bool> isInternalFunc,
            Func<bool> isHideForSite)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;
            this.getDisplayName = getDisplayName;
            this.isProtectedModeFunc = isProtectedModeFunc;
            this.isInternalFunc = isInternalFunc;
            this.isHideForSite = isHideForSite;
        }

        public string Key { get; set; }

        public string ResourceKey { get; set; }

        public string AppPath { get; set; }

        public bool ByPassAuthorization { get; set; }

        public bool ByPassProtectedMode { get; set; }

        public bool ByPassProtectedModeInternal { get; set; }

        public bool IsBeta { get; set; }

        public string DisplayName
        {
            get
            {
                return getDisplayName != null ? getDisplayName() : Alpha_sitemap.ResourceManager.GetString(ResourceKey);
            }
        }

        public virtual string Link
        {
            get
            {
                var appVirtualPath = AppSettings.GetAppPath(AppPath);

                if (!string.IsNullOrWhiteSpace(appVirtualPath))
                {
                    return Regex.Replace(link, HttpRuntime.AppDomainAppVirtualPath, appVirtualPath);
                }

                return link;
            }

            set
            {
                link = value;
            }
        }

        public virtual MenuStatus Status
        {
            get
            {
                var isAuthorized = (ByPassAuthorization
                    || authorizeUrlFunc(Link))
                    && isVisibleForMobileOrDesktop()
                    && !isHideForSite();

                if (isProtectedModeFunc())
                {
                    isAuthorized &= ByPassProtectedMode;

                    if (isInternalFunc())
                    {
                        isAuthorized |= ByPassProtectedModeInternal;
                    }
                }

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
                return SubMenus.Where(menuItem => menuItem.Status.Visible);
            }
        }

        public bool IsNew { get; set; }

        public bool IsOld { get; set; }

        public bool IsMobileView
        {    
            get { return HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice; }
        }

        public bool IsHiddenInMobile { get; set; }

        private bool isVisibleForMobileOrDesktop() => 
            IsMobileView && !IsHiddenInMobile ||
            !IsMobileView;
    }
}
