namespace Alpha.Main.Tests.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using Auth.Services;
    using Helpers;
    using Main.Models.Menus;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MenuItemBuilderImplTests
    {
        private MenuItemBuilderImpl menuItemBuilderImpl;
        private IAuthorizationService authorizationService;
        private IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private Func<bool> isProtectedModeFunc;
        private Func<bool> isInternalFunc;
        private Func<string, string> buildInvalidSessionUniqueKeyFunc;

        [SetUp]
        public void SetUp()
        {
            authorizationService = Substitute.For<IAuthorizationService>();
            menuItemTypeConfiguration = new Dictionary<string, Func<MenuStatus>>
            {
                { string.Empty, null }
            };
            isProtectedModeFunc = () => false;
            isInternalFunc = () => false;
            buildInvalidSessionUniqueKeyFunc = (string url) => string.Empty;

            menuItemBuilderImpl = new MenuItemBuilderImpl(authorizationService, menuItemTypeConfiguration, isProtectedModeFunc, isInternalFunc, buildInvalidSessionUniqueKeyFunc, null, null);
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
                && actualMenuItem.Link == string.Empty
                && actualMenuItem.SubMenus != null
                && !string.IsNullOrEmpty(actualMenuItem.Key));
        }
    }
}