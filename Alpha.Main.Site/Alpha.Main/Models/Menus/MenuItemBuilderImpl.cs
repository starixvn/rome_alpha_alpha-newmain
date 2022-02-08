namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Auth.Services;
    using Auth.Services.Impl;
    using Fanex.Common.Helper;
    using Sunplus.Agent.Model.Entities;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly IDictionary<string, Func<string>> menuItemDisplayName;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;

        public MenuItemBuilderImpl() : this(null, null, null, null, null)
        {
        }

        internal MenuItemBuilderImpl(
            IAuthorizationService authorizationService,
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration,
            IDictionary<string, Func<string>> menuItemDisplayName,
            Func<bool> isProtectedModeFunc,
            Func<bool> isInternalFunc)
        {
            this.authorizationService = authorizationService ?? new AuthorizationService();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
            this.menuItemDisplayName = menuItemDisplayName ?? FunctionRegistry.MenuItemDisplayNameRules;
            this.isProtectedModeFunc = isProtectedModeFunc ?? FunctionRegistry.IsProtectedModeFunc;
            this.isInternalFunc = isInternalFunc ?? FunctionRegistry.IsInternalFunc;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
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
            menuItem.Url = node.Url;

            var useDomainConfigValue = ConfigurationManager.AppSettings[string.Empty + node["UseDomainConfig"]] + string.Empty;
            var mainDomain = HttpContext.Current.Request.Headers["Host"].Replace("mb.", string.Empty).Replace("mb2.", string.Empty);

            useDomainConfigValue = useDomainConfigValue.IndexOf("{0}") != -1 ? string.Format(useDomainConfigValue, mainDomain) : useDomainConfigValue;
            menuItem.UseDomainConfig = useDomainConfigValue;
        }

        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, nameof(node));
            Func<bool> isHideForSite = IsHideForSite(node);

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node["menuItemType"] ?? string.Empty],
                authorizationService.IsAuthorized,
                menuItemDisplayName.ContainsKey(node.ResourceKey) ? menuItemDisplayName[node.ResourceKey] : null,
                isProtectedModeFunc,
                isInternalFunc,
                isHideForSite);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }

        private static Func<bool> IsHideForSite(SiteMapNode node) => () =>
        {
            var hideForSites = node["hideForSites"] == null ? Enumerable.Empty<string>() :
                node["hideForSites"].Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            return hideForSites
                .Any(site => string.Equals(site, "*", StringComparison.OrdinalIgnoreCase) || string.Equals(site, UserSession.Current?.Site, StringComparison.OrdinalIgnoreCase));
        };
    }
}