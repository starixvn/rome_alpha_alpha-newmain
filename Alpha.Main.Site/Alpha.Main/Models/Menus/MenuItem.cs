namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Services;
    using AppResources;
    using Configurations;

    public class MenuItem : IMenuItem
    {
        private string link = string.Empty;
        private Func<MenuStatus> getMenuStatusFunc;
        private Predicate<string> authorizeUrlFunc;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;
        private readonly Func<bool> isHideForSite;
        private readonly Func<string, string> buildInvalidSessionUniqueKeyFunc;
        private readonly Func<string> buildLanguageQueryStringFunc;
        private readonly Func<string> buildDarkModeQueryStringFunc;
        private readonly TransferService transferService;
        private readonly IUser user;

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null)
            : this(getMenuStatusFunc, null, null, null, null, null, null, null, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc)
            : this(getMenuStatusFunc, authorizeUrlFunc, null, null, null, null, null, null, null, null)
        {
        }

        public MenuItem(
            Func<MenuStatus> getMenuStatusFunc,
            Predicate<string> authorizeUrlFunc,
            Func<bool> isProtectedModeFunc,
            Func<bool> isInternalFunc,
            Func<bool> isHideForSite,
            Func<string, string> buildInvalidSessionUniqueKeyFunc,
            Func<string> buildLanguageQueryStringFunc,
            Func<string> buildDarkModeQueryStringFunc,
            TransferService transferService,
            IUser user)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;
            this.isProtectedModeFunc = isProtectedModeFunc;
            this.isInternalFunc = isInternalFunc;
            this.isHideForSite = isHideForSite;
            this.buildInvalidSessionUniqueKeyFunc = buildInvalidSessionUniqueKeyFunc;
            this.buildLanguageQueryStringFunc = buildLanguageQueryStringFunc;
            this.buildDarkModeQueryStringFunc = buildDarkModeQueryStringFunc;
            this.transferService = transferService;
            this.user = user;
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
                return Alpha_sitemap.ResourceManager.GetString(ResourceKey);
            }
        }

        public virtual string Link
        {
            get
            {
                var context = HttpContext.Current;
                var session = context?.Session;
                var appVirtualPath = AppSettings.GetAppPath(AppPath) ?? string.Empty;
                var runtimeAppPath = HttpRuntime.AppDomainAppVirtualPath ?? string.Empty;
                var reportUrl = $"{appVirtualPath}{context?.Response?.ApplyAppPathModifier(runtimeAppPath)}{link}";
                reportUrl = !string.IsNullOrWhiteSpace(runtimeAppPath) ? reportUrl.Replace(runtimeAppPath, string.Empty) : reportUrl;

                var cookieless = session != null && session.IsCookieless ?
                    $"/(S({session?.SessionID}))" : string.Empty;

                var menuLink = !string.IsNullOrWhiteSpace(UseDomainConfig) ? $"{UseDomainConfig}{cookieless}{link}" :
                    appVirtualPath.Equals("/site-reports") || appVirtualPath.Equals("/site-betlists") ? reportUrl :
                    !string.IsNullOrWhiteSpace(link) && !string.IsNullOrWhiteSpace(runtimeAppPath) ?
                    link.Replace(runtimeAppPath, appVirtualPath) : string.Empty;

                var invalidSessionUniqueKey = buildInvalidSessionUniqueKeyFunc(menuLink);

                if (!string.IsNullOrWhiteSpace(link) && string.Equals("transfer", ResourceKey, StringComparison.OrdinalIgnoreCase))
                {
                    menuLink = string.Format(menuLink, transferService.IsAllDayBalanceTransfer(user.CustId));
                }

                if ((menuLink.IndexOf("PTTemplate/Index", StringComparison.OrdinalIgnoreCase) > -1)
                    || (menuLink.IndexOf("Customer/CreateDirectMember", StringComparison.OrdinalIgnoreCase) > -1))
                {
                    var languageParam = buildLanguageQueryStringFunc();
                    var darkModeParam = buildDarkModeQueryStringFunc();
                    return string.IsNullOrWhiteSpace(invalidSessionUniqueKey)
                    ? $"{menuLink}?{languageParam}&{darkModeParam}"
                    : $"{menuLink}?ssguid={invalidSessionUniqueKey}&{languageParam}&{darkModeParam}";
                }

                return string.IsNullOrWhiteSpace(invalidSessionUniqueKey)
                    ? menuLink
                    : $"{menuLink}?ssguid={invalidSessionUniqueKey}";
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

        public bool IsIncludeLanguage { get; set; }

        public bool IsMobileView
        {
            get { return HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice; }
        }

        public bool IsHiddenInMobile { get; set; }

        public string UseDomainConfig { get; set; }

        private bool isVisibleForMobileOrDesktop() =>
            IsMobileView && !IsHiddenInMobile ||
            !IsMobileView;
    }
}