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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", 
            MessageId = "0", Justification = "Reviewed!")]
        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, nameof(node));

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node["menuItemType"] ?? string.Empty],
               authorizationService.IsAuthorized);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }
    }
}