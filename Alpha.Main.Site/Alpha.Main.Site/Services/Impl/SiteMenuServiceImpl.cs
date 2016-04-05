namespace Alpha.Main.Site.Services.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using Fanex.Common.Helper;
    using Helpers;
    using Models;
    using Models.Enumerations;
    using Models.Menus;

    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly CachingService cacheManager;
        private readonly MenuItemBuilder menuItemBuilder;

        public SiteMenuServiceImpl(CachingService cacheManager, MenuItemBuilder menuItemBuilder)
        {
            Check.Argument.IsNotNull(cacheManager, nameof(cacheManager));
            Check.Argument.IsNotNull(menuItemBuilder, nameof(menuItemBuilder));

            this.cacheManager = cacheManager;
            this.menuItemBuilder = menuItemBuilder;
        }

        public IEnumerable<IMenuItem> GetMenuByUser(IUser user)
        {
            Check.Argument.IsNotNull(user, nameof(user));

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
                Check.Argument.IsNotNull(SiteMap.RootNode, nameof(SiteMap.RootNode));

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
                            var menuItem = menuItemBuilder.BuildMenuItem(node);

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