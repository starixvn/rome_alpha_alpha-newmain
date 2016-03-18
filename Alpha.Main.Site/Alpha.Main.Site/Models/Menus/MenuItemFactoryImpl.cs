namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.Common.Helper;
    using SimpleInjector;

    public class MenuItemFactoryImpl : MenuItemFactory
    {
        private readonly Container container;

        public MenuItemFactoryImpl(Container container)
        {
            this.container = container;
        }

        private static Type GetMenuItemType(SiteMapNode siteMapNode)
        {
            var menuItemType = string.IsNullOrWhiteSpace(siteMapNode["menuItemType"])
                ? typeof(MenuItem)
                : Type.GetType($"Alpha.Main.Site.Models.Menus.{siteMapNode["menuItemType"]}", throwOnError: false, ignoreCase: false);

            return menuItemType;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.ResourceKey;
            menuItem.Name = node.ResourceKey.ToLower();
            menuItem.Url = node.Url;
            menuItem.AppName = node["AppName"];
            menuItem.SubMenus = new List<IMenuItem>();           

            if (node.HasChildNodes)
            {
                menuItem.ShowSubMenus = GetExpandAttributeInfo(node);
            }
        }

        private static bool GetExpandAttributeInfo(SiteMapNode node)
        {
            return !string.IsNullOrWhiteSpace(node["expand"])
                   && ("1".Equals(node["expand"])
                     || "true".Equals(node["expand"], StringComparison.InvariantCultureIgnoreCase));
        }

        public IMenuItem Create(SiteMapNode siteMapNode)
        {
            Contract.Ensures(siteMapNode != null);
                        
            var menuItem = (IMenuItem)this.container.GetInstance(GetMenuItemType(siteMapNode));

            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }
    }
}