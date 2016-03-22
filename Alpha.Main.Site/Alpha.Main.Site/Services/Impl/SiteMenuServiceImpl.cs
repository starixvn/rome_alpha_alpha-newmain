namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;
    using Models;
    using Models.Dtos;
    using Models.Enumerations;
    using Models.Menus;

    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly CachingService cacheManager;
        private readonly MenuItemFactory menuItemFactory;

        public SiteMenuServiceImpl(CachingService cacheManager, MenuItemFactory menuItemFactory)
        {
            Contract.Ensures(cacheManager != null, nameof(cacheManager));
            Contract.Ensures(menuItemFactory != null, nameof(menuItemFactory));

            this.cacheManager = cacheManager;
            this.menuItemFactory = menuItemFactory;
        }

        public IEnumerable<IMenuItem> GetMenuByUser(IUser user)
        {
            Contract.Ensures(user != null, nameof(user));

            var menuItems = Enumerable.Empty<IMenuItem>();

            menuItems = GetMenuDataByUserLevel(user.Level);

            return menuItems;
        }

        private List<IMenuItem> GetMenuDataByUserLevel(UserLevel userLevel)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;

            List<IMenuItem> menuItems = cacheManager.Get<List<IMenuItem>>(cacheKey);

            if (menuItems == null)
            {
                Contract.Requires(SiteMap.RootNode != null, nameof(SiteMap.RootNode));

                menuItems = BuildMenu(SiteMap.RootNode, userLevel);

                cacheManager.Add(cacheKey, menuItems);
            }

            return menuItems;
        }

        private List<IMenuItem> BuildMenu(SiteMapNode siteMapNode, UserLevel userLevel)
        {
            Contract.Ensures(siteMapNode != null, nameof(siteMapNode));

            var menuItems = new List<IMenuItem>();

            if (siteMapNode.HasChildNodes)
            {
                siteMapNode.ChildNodes.Cast<SiteMapNode>().ToList().ForEach(
                    node =>
                    {
                        if (node.Roles.Count == 0
                            || node.Roles.Contains(userLevel.DisplayName)
                            || node.Roles.Contains(userLevel.DisplayName.ToLower())
                            || node.Roles.Contains(userLevel.Value.ToString()))
                        {
                            var menuItem = menuItemFactory.Create(node);

                            if (node.HasChildNodes)
                            {
                                menuItem.SubMenus = BuildMenu(node, userLevel);
                            }

                            menuItems.Add(menuItem);
                        }
                    });
            }

            return menuItems;
        }
    }
}