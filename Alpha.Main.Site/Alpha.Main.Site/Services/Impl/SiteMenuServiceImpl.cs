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

    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly CachingService cacheManager;

        public SiteMenuServiceImpl(CachingService cacheManager)
        {
            this.cacheManager = cacheManager ?? new CachingServiceImpl();
        }

        public IEnumerable<MenuItem> GetMenuByUser(IUser user)
        {
            var menuItems = Enumerable.Empty<MenuItem>();

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

        private IEnumerable<MenuItem> GetMenuByUserLevel(UserLevel userLevel)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;
            IEnumerable<MenuItem> menuItems = cacheManager.Get<IEnumerable<MenuItem>>(cacheKey);

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

            return menuItems;
        }

        private List<MenuItem> BuildMenu(SiteMapNode siteMapNode, UserLevel userLevel)
        {
            var menuItems = new List<MenuItem>();

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
                            MenuItem menuItem = BuildMenuItem(node);

                            if (node.HasChildNodes)
                            {
                                menuItem.SubMenus = BuildMenu(node, userLevel);
                                menuItem.ShowSubMenus = GetExpandAttributeInfo(node);
                            }

                            menuItems.Add(menuItem);
                        }
                    });
            }

            return menuItems;
        }

        private static MenuItem BuildMenuItem(SiteMapNode node)
        {
            var menuItem = new MenuItem(node.Key);
            menuItem.Key = node.ResourceKey;
            menuItem.Name = node.ResourceKey.ToLower();
            menuItem.Url = node.Url;
            menuItem.SubMenus = new List<MenuItem>();
            menuItem.Function = node["functionType"];

            return menuItem;
        }

        private static bool GetExpandAttributeInfo(SiteMapNode node)
        {
           return !string.IsNullOrWhiteSpace(node["expand"])
                  && ("1".Equals(node["expand"]) 
                    || "true".Equals(node["expand"], StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<MenuItem> GetMenuForSubAccount(
            IUser user,
            IEnumerable<MenuItem> userLevelMenu)
        {
            var subAccountMenu = userLevelMenu
                .Where(menuItem => menuItem.IsAccessibleToUser(user));

            return subAccountMenu;
        }
    }
}