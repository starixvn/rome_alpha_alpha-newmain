namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using App_Start;
    using Fanex.Common.Helper;
    using Services;
    using Services.Impl;
    using SimpleInjector;

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
            Contract.Ensures(siteMapNode != null);        

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[siteMapNode["menuItemType"] ?? string.Empty], 
               authorizeUrlService.CanAccessUrl);

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }        
    }
}