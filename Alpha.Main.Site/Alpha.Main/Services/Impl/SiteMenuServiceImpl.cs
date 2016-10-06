namespace Alpha.Main.Services.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using Fanex.Common.Helper;
    using Helpers;
    using Models.Enumerations;
    using Models.Menus;

    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly CachingManager cacheManager;
        private readonly MenuItemBuilder menuItemBuilder;
        private readonly SiteMapReader siteMapReader;

        public SiteMenuServiceImpl(
            CachingManager cacheManager,
            MenuItemBuilder menuItemBuilder,
            SiteMapReader siteMapReader)
        {
            Check.Argument.IsNotNull(cacheManager, nameof(cacheManager));
            Check.Argument.IsNotNull(menuItemBuilder, nameof(menuItemBuilder));
            Check.Argument.IsNotNull(siteMapReader, nameof(siteMapReader));

            this.cacheManager = cacheManager;
            this.menuItemBuilder = menuItemBuilder;
            this.siteMapReader = siteMapReader;
        }

        public IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false)
        {
            var menuItems = Enumerable.Empty<IMenuItem>();

            menuItems = GetMenuDataByUserLevel(userLevel, isCookieLess);

            return menuItems;
        }

        private List<IMenuItem> GetMenuDataByUserLevel(UserLevel userLevel, bool isCookieLess)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;

            List<IMenuItem> menuItems = cacheManager.Get<List<IMenuItem>>(cacheKey);

            if (menuItems == null || isCookieLess)
            {
                var rootNode = siteMapReader.GetRootNode();

                menuItems = BuildMenu(rootNode, userLevel);

                if (!isCookieLess)
                {
                    cacheManager.Add(cacheKey, menuItems);
                }
            }

            return menuItems;
        }

        private List<IMenuItem> BuildMenu(SiteMapNode siteMapNode, UserLevel userLevel)
        {
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