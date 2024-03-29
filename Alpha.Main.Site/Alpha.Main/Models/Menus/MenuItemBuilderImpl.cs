namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Alpha.Auth.Services;
    using Alpha.Auth.Services.Impl;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using AlphaPlus.User.Models;
    using Fanex.Common.Helper;
    using Fanex.Data.Repository;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;
        private readonly Func<string, string> buildInvalidSessionUniqueKeyFunc;
        private readonly Func<string> buildLanguageQueryStringFunc;
        private readonly Func<string> buildDarkModeQueryStringFunc;
        private readonly TransferService transferService;
        private readonly IUser user;

        public MenuItemBuilderImpl() : this(null, null, null, null, null, null, null, null, null)
        {
        }

        internal MenuItemBuilderImpl(
            IAuthorizationService authorizationService,
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration,
            Func<bool> isProtectedModeFunc,
            Func<bool> isInternalFunc,
            Func<string, string> buildInvalidSessionUniqueKeyFunc,
            Func<string> buildLanguageQueryStringFunc,
            Func<string> buildDarkModeQueryStringFunc,
            TransferService transferService,
            IUser user)
        {
            this.authorizationService = authorizationService ?? new AuthorizationService();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
            this.isProtectedModeFunc = isProtectedModeFunc ?? FunctionRegistry.IsProtectedModeFunc;
            this.isInternalFunc = isInternalFunc ?? FunctionRegistry.IsInternalFunc;
            this.buildInvalidSessionUniqueKeyFunc = buildInvalidSessionUniqueKeyFunc ?? FunctionRegistry.BuildInvalidSessionUniqueKey;
            this.transferService = transferService ?? new TransferServiceImpl(new DynamicRepository());
            this.user = user ?? new User();
            this.buildLanguageQueryStringFunc = buildLanguageQueryStringFunc ?? FunctionRegistry.BuildLanguageQueryString;
            this.buildDarkModeQueryStringFunc = buildDarkModeQueryStringFunc ?? FunctionRegistry.BuildDarkModeQueryString;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            var useDomainConfigValue = ConfigurationManager.AppSettings[string.Empty + node["UseDomainConfig"]] + string.Empty;
            var mainDomain = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Headers["Host"];

            if (HttpContext.Current != null && HttpContext.Current.Session.IsCookieless)
            {
                var subDomain = mainDomain.Split('.').ElementAt(0);
                mainDomain = mainDomain.Replace($"{subDomain}.", string.Empty);
            }

            useDomainConfigValue = useDomainConfigValue.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) != -1 ? string.Format(useDomainConfigValue, mainDomain) : useDomainConfigValue;

            menuItem.UseDomainConfig = useDomainConfigValue;
            menuItem.Key = node.Key;
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Link = node.Url;
            menuItem.AppPath = node["AppPath"];
            menuItem.IsBeta = node["isBeta"] != null && bool.Parse(node["isBeta"]);
            menuItem.IsNew = node["isNew"] != null && bool.Parse(node["isNew"]);
            menuItem.IsOld = node["isOld"] != null && bool.Parse(node["isOld"]);
            menuItem.IsHiddenInMobile = node["isHiddenInMobile"] != null && bool.Parse(node["isHiddenInMobile"]);
            menuItem.ByPassAuthorization = node["byPassAuthorization"] == null ? false : bool.Parse(node["byPassAuthorization"]);
            menuItem.ByPassProtectedMode = node["byPassProtectedMode"] == null ? false : bool.Parse(node["byPassProtectedMode"]);
            menuItem.ByPassProtectedModeInternal = node["byPassProtectedModeInternal"] == null ? false : bool.Parse(node["byPassProtectedModeInternal"]);
            menuItem.SubMenus = new List<IMenuItem>();
        }

        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, nameof(node));
            Func<bool> isShowForSite = IsShowForSite(node);

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node["menuItemType"] ?? string.Empty],
                authorizationService.IsAuthorized,
                isProtectedModeFunc,
                isInternalFunc,
                isShowForSite,
                buildInvalidSessionUniqueKeyFunc,
                buildLanguageQueryStringFunc,
                buildDarkModeQueryStringFunc,
                transferService,
                user);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }

        private static Func<bool> IsShowForSite(SiteMapNode node) => () =>
        {
            var showForSites = node["showForSites"] == null ? Enumerable.Empty<string>() :
                node["showForSites"].Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return !showForSites.Any() ||
                showForSites.Any(site => string.Equals(site, UserSession.Current?.Site, StringComparison.OrdinalIgnoreCase));
        };
    }
}