namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Helpers;
    using Helps;
    using Main.Services;
    using Main.Services.Impl;
    using Models.Enumerations;
    using Models.Menus;
    using NSubstitute;
    using NUnit.Framework;

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
        public void GetMenuByUserLevel_UserLevelIsValid_ReturnMenuItemsByUserLevel(byte userLevelId, int numberOfAccessedMenu)
        {
            //// Arrange
            var rootNode = CreateRootNodeStub(@"\TestData\Menus\TestGetByLevel.sitemap");
            siteMapReader.GetRootNode().Returns(rootNode);

            //// Act
            var actualMenuItems = siteMenuServiceImpl.GetMenuByUserLevel(Enumeration.FromValue<UserLevel>(userLevelId));

            //// Assert
            Assert.AreEqual(numberOfAccessedMenu, CountMenuItems(actualMenuItems));
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

        private SiteMapNode CreateRootNodeStub(string siteMapFilePath)
        {
            //var rootNode = SiteMap.RootNode;
            var customeSiteMapProvider = new CustomSiteMapProvider();
            NameValueCollection providerAttributes = new NameValueCollection(1);
            providerAttributes.Add("siteMapFile", siteMapFilePath);
            // Initialize the provider with a provider name and file name.
            customeSiteMapProvider.Initialize("testProvider", providerAttributes);
            var rootNode = customeSiteMapProvider.BuildSiteMap();

            return rootNode;
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
