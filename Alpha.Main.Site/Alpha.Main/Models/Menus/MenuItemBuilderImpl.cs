namespace Alpha.Main.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Auth.Services;
    using Auth.Services.Impl;
    using Fanex.Common.Helper;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly IDictionary<string, Func<string>> menuItemDisplayName;

        public MenuItemBuilderImpl() : this(null, null, null)
        {
        }

        internal MenuItemBuilderImpl(
            IAuthorizationService authorizationService,
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration,
            IDictionary<string, Func<string>> menuItemDisplayName)
        {
            this.authorizationService = authorizationService ?? new AuthorizationService();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
            this.menuItemDisplayName = menuItemDisplayName ?? FunctionRegistry.MenuItemDisplayNameRules;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.Key;
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Link = node.Url;
            menuItem.AppPath = node["AppPath"];
            menuItem.IsBeta = node["isBeta"] != null && bool.Parse(node["isBeta"]);
            menuItem.IsNew = node["isNew"] != null && bool.Parse(node["isNew"]);
            menuItem.ByPassAuthorization = node["byPassAuthorization"] == null ? false : bool.Parse(node["byPassAuthorization"]);
            menuItem.SubMenus = new List<IMenuItem>();
        }

        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, nameof(node));

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node["menuItemType"] ?? string.Empty],
                authorizationService.IsAuthorized,
                menuItemDisplayName.ContainsKey(node.ResourceKey) ? menuItemDisplayName[node.ResourceKey] : null);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }
    }
}