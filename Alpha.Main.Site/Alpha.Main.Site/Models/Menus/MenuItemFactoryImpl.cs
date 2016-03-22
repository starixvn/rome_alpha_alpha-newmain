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

    public class MenuItemFactoryImpl : MenuItemFactory
    {
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly AuthorizeUrlService authorizeUrlService;

        public MenuItemFactoryImpl() : this(null, null)
        {
        }

        internal MenuItemFactoryImpl(AuthorizeUrlService authorizeUrlService, IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration)
        {
            this.authorizeUrlService = authorizeUrlService ?? new AuthorizeUrlServiceImpl();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? MenuItemRulesConfig.MenuItemTypeStatuRules;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.ResourceKey;
            menuItem.Name = node.ResourceKey.ToLower();
            menuItem.Url = node.Url;
            menuItem.AppName = node["AppName"];
            menuItem.SubMenus = new List<IMenuItem>();
        }

        public IMenuItem Create(SiteMapNode siteMapNode)
        {
            Contract.Ensures(siteMapNode != null); ;

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[siteMapNode["menuItemType"] ?? string.Empty], 
                (string s) => authorizeUrlService.CanAccessUrl(s));

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }
    }
}