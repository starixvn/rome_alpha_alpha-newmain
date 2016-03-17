namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
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
            this.cacheManager = cacheManager ?? new CachingServiceImpl();
            this.menuItemFactory = menuItemFactory ?? new MenuItemFactoryImpl();
        }

        public IEnumerable<IMenuItem> GetMenuByUser(IUser user)
        {
            var menuItems = Enumerable.Empty<IMenuItem>();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            menuItems = GetMenuByUserLevel(user.Level);

            if (user.IsSubAccount)
            {
                menuItems = GetMenuForSubAccount(user, menuItems);
            }

            return menuItems;
        }

        private IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;
            IEnumerable<IMenuItem> menuItems = cacheManager.Get<IEnumerable<IMenuItem>>(cacheKey);

            if (menuItems == null)
            {
                var rootNode = SiteMap.RootNode;

                if (rootNode == null)
                {
                    throw new InvalidOperationException(nameof(rootNode));
                }

                menuItems = BuildMenu(rootNode, userLevel);

                cacheManager.Add(cacheKey, menuItems);
            }

            return menuItems.Where(menuItem => menuItem.IsVisible);
        }

        private List<IMenuItem> BuildMenu(SiteMapNode siteMapNode, UserLevel userLevel)
        {
            var menuItems = new List<IMenuItem>();

            if (ReferenceEquals(null, siteMapNode))
            {
                throw new ArgumentNullException(nameof(siteMapNode));
            }

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

        private IEnumerable<IMenuItem> GetMenuForSubAccount(
            IUser user,
            IEnumerable<IMenuItem> userLevelMenu)
        {
            var subAccountMenu = userLevelMenu
                .Where(menuItem => menuItem.IsVisible);

            return subAccountMenu;
        }
    }
}