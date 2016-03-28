namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using App_Start;
    using Helpers;
    using Services;
    using Services.Impl;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly AuthorizeUrlService authorizeUrlService;

        public MenuItemBuilderImpl() : this(null, null)
        {
        }

        internal MenuItemBuilderImpl(
            AuthorizeUrlService authorizeUrlService, 
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration)
        {
            this.authorizeUrlService = authorizeUrlService ?? new AuthorizeUrlServiceImpl();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.Key;
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Url = node.Url;
            menuItem.AppName = node["AppName"];
            menuItem.SubMenus = new List<IMenuItem>();
        }

        public IMenuItem BuildMenuItem(SiteMapNode siteMapNode)
        {            
            Argument.Requires(siteMapNode != null);        

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[siteMapNode["menuItemType"] ?? string.Empty], 
               authorizeUrlService.CanAccessUrl);

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }        
    }
}