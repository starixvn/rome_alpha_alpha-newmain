namespace Alpha.Main.Site.Tests.Handlers
{
    using Main.Models.Enumerations;
    using Main.Models.Menus;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class MenuBarHandlerTests
    {
        private SiteMenuService siteMenuService;
        private MenuBarHandler menuBarHandler;

        [SetUp]
        public void SetUp()
        {
            siteMenuService = Substitute.For<SiteMenuService>();
            menuBarHandler = new MenuBarHandler(siteMenuService);
        }

        [Test]
        public void HandleMenuBarQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            MenuBarQuery nullQuery = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => menuBarHandler.Handle(nullQuery)));
        }

        [Test]
        public void HandleMenuBarQuery_WhenCalled_ExecuteGetMenuByUserLevel()
        {
            //// Arrange
            var menuBarQuery = new MenuBarQuery(2, "userAgent");

            //// Act
            menuBarHandler.Handle(menuBarQuery);

            //// Assert
            siteMenuService.Received().GetMenuByUserLevel(
                Arg.Is<UserLevel>(
                    userLevel => userLevel.Value == menuBarQuery.UserLevel.Value));
        }

        [Test]
        public void HandleMenuBarQuery_WhenCalled_ReturnMenuBarViewModel()
        {
            //// Arrange
            var menuBarQuery = new MenuBarQuery(2, "userAgent");
            var unauthorizedUrlFunction = Substitute.For<Predicate<string>>();
            unauthorizedUrlFunction.Invoke(Arg.Any<string>()).Returns(true);

            var getMenuStatusFunc = Substitute.For<Func<MenuStatus>>();
            getMenuStatusFunc.Invoke().Returns(new MenuStatus(true, false));

            siteMenuService.GetMenuByUserLevel(Arg.Any<UserLevel>()).Returns(new List<MenuItem>
            {
                new MenuItem(getMenuStatusFunc, unauthorizedUrlFunction),
                new MenuItem(getMenuStatusFunc, unauthorizedUrlFunction)
            });

            //// Act
            var menuBarViewModel = menuBarHandler.Handle(menuBarQuery);

            //// Assert
            Assert.IsTrue(
                menuBarViewModel.MenuItems.Count() == 2
                && menuBarViewModel.IsSmallDevice == menuBarQuery.IsSmallDevice);
        }
    }
}