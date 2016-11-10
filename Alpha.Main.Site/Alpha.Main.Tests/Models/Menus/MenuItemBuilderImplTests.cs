namespace Alpha.Main.Tests.Models.Menus
{
    using Auth.Services;
    using Helpers;
    using Main.Models.Menus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class MenuItemBuilderImplTests
    {
        private MenuItemBuilderImpl menuItemBuilderImpl;
        private IAuthorizationService authorizationService;
        private IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private IDictionary<string, Func<string>> menuItemDisplayName;

        [SetUp]
        public void SetUp()
        {
            authorizationService = Substitute.For<IAuthorizationService>();
            menuItemTypeConfiguration = new Dictionary<string, Func<MenuStatus>>
            {
                { string.Empty, null }
            };
            menuItemDisplayName = new Dictionary<string, Func<string>>
            {
                { string.Empty, null }
            };

            menuItemBuilderImpl = new MenuItemBuilderImpl(authorizationService, menuItemTypeConfiguration, menuItemDisplayName);
        }

        [Test]
        public void BuildMenuItem_SiteMapNodeIsNull_ThrowArgumentIsNullException()
        {
            Assert.Throws<ArgumentNullException>(() => menuItemBuilderImpl.BuildMenuItem(null));
        }

        [Test]
        public void BuildMenuItem_ReadDataFromSiteMapNode_ReturnMenuItem()
        {
            /// Arrange
            var rootNode = CustomSiteMapProvider.CreateRootNodeStub(@"\TestData\Menus\MenuItemBuilder.sitemap");
            var testNode = rootNode.ChildNodes[0];

            // Act
            var actualMenuItem = menuItemBuilderImpl.BuildMenuItem(testNode);

            // Assert
            Assert.IsTrue(
                actualMenuItem.ResourceKey == "soccer"
                && actualMenuItem.AppPath == "TotalbetsForecastSiteVirtualPath"
                && actualMenuItem.Url == "~/Forecast"
                && actualMenuItem.SubMenus != null
                && !string.IsNullOrEmpty(actualMenuItem.Key));
        }
    }
}