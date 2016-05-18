namespace Alpha.Main.Tests.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Menus;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MenuItemTests
    {
        private MenuItem menuItem;

        [Test]
        public void VisibileSubMenus_SubMenuItemsHasData_ReturnAllVisibledSubMenuItems()
        {
            //// Arrange
            var authorizedUrlFunction = Substitute.For<Predicate<string>>();
            authorizedUrlFunction.Invoke(Arg.Any<string>()).Returns(true);

            var unauthorizedUrlFunction = Substitute.For<Predicate<string>>();
            unauthorizedUrlFunction.Invoke(Arg.Any<string>()).Returns(false);

            var getMenuStatusFunc = Substitute.For<Func<MenuStatus>>();
            getMenuStatusFunc.Invoke().Returns(new MenuStatus(true, false));

            //// Act
            menuItem = new MenuItem
            {
                SubMenus = new List<IMenuItem>
                {
                    new MenuItem(getMenuStatusFunc, authorizedUrlFunction),
                    new MenuItem(getMenuStatusFunc, unauthorizedUrlFunction)
                }
            };

            //// Assert
            Assert.AreEqual(1, menuItem.VisibileSubMenus.Count());
        }

        [Test]
        public void Status_UserIsNotAuthorizedOnMenu_ReturnStatusIsNotVisible()
        {
            //// Arrange
            var authorizedUrlFunction = Substitute.For<Predicate<string>>();
            authorizedUrlFunction.Invoke(Arg.Any<string>()).Returns(false);

            //// Act
            var actualMenuItem = new MenuItem(null, authorizedUrlFunction);

            //// Assert
            Assert.IsFalse(actualMenuItem.Status.Visible);
        }

        [Test]
        public void Status_UserIsAuthorizedOnMenu_ExecuteGetMenuStatusFunc()
        {
            //// Arrange
            var authorizedUrlFunction = Substitute.For<Predicate<string>>();
            authorizedUrlFunction.Invoke(Arg.Any<string>()).Returns(true);
            var getMenuStatusFunc = Substitute.For<Func<MenuStatus>>();

            //// Act
            var actualMenuItemStatus = new MenuItem(getMenuStatusFunc, authorizedUrlFunction).Status;

            //// Assert
            getMenuStatusFunc.Received().Invoke();
        }
    }
}