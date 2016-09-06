namespace Alpha.Main.Tests.Services
{
    using Helpers;
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Main.Models.Menus;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Web;

    [TestFixture]
    public class SiteMenuServiceImplTests
    {
        private CachingManager cachingManager;
        private MenuItemBuilder menuItemBuilder;
        private SiteMapReader siteMapReader;

        private SiteMenuServiceImpl siteMenuServiceImpl;

        [SetUp]
        public void SetUp()
        {
            cachingManager = Substitute.For<CachingManager>();
            menuItemBuilder = Substitute.For<MenuItemBuilder>();
            siteMapReader = Substitute.For<SiteMapReader>();

            siteMenuServiceImpl = new SiteMenuServiceImpl(cachingManager, menuItemBuilder, siteMapReader);
        }

        [TestCase(2, "Agent")]
        [TestCase(3, "Master")]
        [TestCase(4, "Super")]
        public void GetMenuByUserLevel_CacheHasSiteMapData_ReturnDataFromCache(byte userLevelId, string userLevelName)
        {
            //// Arrange
            var menuItems = CreateMenuItemsStub();
            cachingManager.Get<List<IMenuItem>>($"Alpha.Main.MenuForLevel.{userLevelName}").Returns(menuItems);

            //// Act
            var actualMenuItems = siteMenuServiceImpl.GetMenuByUserLevel(Enumeration.FromValue<UserLevel>(userLevelId));

            //// Assert
            Assert.AreEqual(menuItems, actualMenuItems);
        }

        [TestCase(2, 5)] //// Agent
        [TestCase(3, 7)] //// Master
        [TestCase(4, 6)] //// Super
        public void GetMenuByUserLevel_UserLevelIsValid_ReturnMenuItemsByUserLevelName(byte userLevelId, int numberOfAccessedMenu)
        {
            //// Arrange
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\SiteMapByLevelName.sitemap");
            siteMapReader.GetRootNode().Returns(rootNode);

            //// Act
            var actualMenuItems = siteMenuServiceImpl.GetMenuByUserLevel(Enumeration.FromValue<UserLevel>(userLevelId));

            //// Assert
            Assert.AreEqual(numberOfAccessedMenu, CountMenuItems(actualMenuItems));
        }

        [TestCase(2, 5)] //// Agent
        [TestCase(3, 7)] //// Master
        [TestCase(4, 6)] //// Super
        public void GetMenuByUserLevel_UserLevelIsValid_ReturnMenuItemsByUserLevelId(byte userLevelId, int numberOfAccessedMenu)
        {
            //// Arrange
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\SiteMapByLevelId.sitemap");
            siteMapReader.GetRootNode().Returns(rootNode);

            //// Act
            var actualMenuItems = siteMenuServiceImpl.GetMenuByUserLevel(Enumeration.FromValue<UserLevel>(userLevelId));

            //// Assert
            Assert.AreEqual(numberOfAccessedMenu, CountMenuItems(actualMenuItems));
        }

        [Test]
        public void GetMenuByUserLevel_ReadSiteMapData_ExecuteBuildMenuItemForEachMenu()
        {
            //// Arrange
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\OneUrlAgentSiteMap.sitemap");
            siteMapReader.GetRootNode().Returns(rootNode);

            //// Act
            siteMenuServiceImpl.GetMenuByUserLevel(Enumeration.FromValue<UserLevel>(2));

            //// Assert
            menuItemBuilder.Received(1).BuildMenuItem(
                Arg.Is<SiteMapNode>(siteMapNode => siteMapNode.Url == "~/Forecast"
                                                && siteMapNode.ResourceKey == "soccer"));
        }

        [TestCase(2, 5)] //// Agent
        [TestCase(3, 7)] //// Master
        [TestCase(4, 6)] //// Super
        public void GetMenuByUserLevel_GotSiteMapData_StoreSiteMapInCacheByUserLevel(byte userLevelId, int numberOfAccessedMenu)
        {
            //// Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\SiteMapByLevelName.sitemap");
            siteMapReader.GetRootNode().Returns(rootNode);

            //// Act
            siteMenuServiceImpl.GetMenuByUserLevel(userLevel);

            //// Assert
            cachingManager.Received().Add(
                $"Alpha.Main.MenuForLevel.{userLevel.DisplayName}",
                Arg.Is<IEnumerable<IMenuItem>>(
                menuItems => CountMenuItems(menuItems) == numberOfAccessedMenu));
        }

        private int CountMenuItems(IEnumerable<IMenuItem> menuItems)
        {
            var totalCount = 0;

            foreach (var menuItem in menuItems)
            {
                totalCount++;
                if (menuItem.SubMenus != null)
                {
                    totalCount += CountMenuItems(menuItem.SubMenus);
                }
            }

            return totalCount;
        }

        private List<IMenuItem> CreateMenuItemsStub()
        {
            return new List<IMenuItem>
            {
                new MenuItem
                {
                    Key = "a",
                    Url = "~/Url",
                    ShowSubMenus = true,
                    AppPath = "~/site-main",
                    SubMenus = new List<IMenuItem>
                    {
                        new MenuItem
                        {
                            Key = "a",
                            Url = "~/Url",
                            ShowSubMenus = true,
                            AppPath = "~/site-main",
                        }
                    }
                }
            };
        }
    }
}