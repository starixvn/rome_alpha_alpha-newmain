namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using App_Start;
    using Auth.Services;
    using Auth.Services.Impl;
    using Helpers;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly IAuthorizationService authorizationService;

        public MenuItemBuilderImpl() : this(null, null)
        {
        }

        internal MenuItemBuilderImpl(
            IAuthorizationService authorizationService,
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration)
        {
            this.authorizationService = authorizationService ?? new AuthorizationService();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.Key;
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Url = node.Url;
            menuItem.AppPath = node["AppPath"];
            menuItem.SubMenus = new List<IMenuItem>();
        }

        public IMenuItem BuildMenuItem(SiteMapNode siteMapNode)
        {
            Argument.Requires(siteMapNode != null);

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[siteMapNode["menuItemType"] ?? string.Empty],
               authorizationService.IsAuthorized);

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }
    }
}