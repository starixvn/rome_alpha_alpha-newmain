namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using App_Start;
    using Fanex.Common.Helper;
    using SimpleInjector;

    public class MenuItemFactoryImpl : MenuItemFactory
    {
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration; 

        public MenuItemFactoryImpl() : this(null)
        {
        }

        internal MenuItemFactoryImpl(IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration)
        {
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? MenuItemRulesConfig.MenuItemTypeStatuRules;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.ResourceKey;
            menuItem.Name = node.ResourceKey.ToLower();
            menuItem.Url = node.Key;
            menuItem.AppName = node["AppName"];
            menuItem.SubMenus = new List<IMenuItem>();          
        }

        public IMenuItem Create(SiteMapNode siteMapNode)
        {
            Contract.Ensures(siteMapNode != null);

            var menuItem = new MenuItem(menuItemTypeConfiguration[siteMapNode["menuItemType"] ?? string.Empty]);

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }
    }
}