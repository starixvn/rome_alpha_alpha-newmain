namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.Common.Helper;

    public class MenuItemFactoryImpl : MenuItemFactory
    {
        public IMenuItem Create(SiteMapNode siteMapNode)
        {
            Check.Argument.IsNotNull(siteMapNode, nameof(siteMapNode));

            var type = GetMenuItemType(siteMapNode);
            var menuItem = CreateMenuInstance(type);
            SetDataForMenuItem(menuItem, siteMapNode);

            return menuItem;
        }

        private static IMenuItem CreateMenuInstance(Type menuItemType)
        {
            IMenuItem menuItem = (menuItemType != null)
               ? (IMenuItem)Activator.CreateInstance(menuItemType)
               : new MenuItem();

            return menuItem;
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
            menuItem.Function = node["functionType"];

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
    }
}